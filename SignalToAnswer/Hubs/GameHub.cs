﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using SignalToAnswer.Attributes;
using SignalToAnswer.Constants;
using SignalToAnswer.Dtos.GameHub;
using SignalToAnswer.Entities;
using SignalToAnswer.Exceptions;
using SignalToAnswer.Extensions;
using SignalToAnswer.Mappers.Dtos.GameHub;
using SignalToAnswer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalToAnswer.Hubs
{
    public interface IGameHub
    {
        public Task ReceiveLoadingMessage(string message);

        public Task ReceiveHideLoading();

        public Task ReceiveResultInfo(List<ResultInfoDto> dtos);

        public Task ReceiveQuestionInfo(QuestionInfoDto dto);

        public Task ReceiveAnswerChoice(List<AnswerChoiceDto> dto);

        public Task ReceiveTimerInfo(int time);

        public Task ReceiveGameCancelled(string message);
    }

    [Transactional]
    [Authorize]
    public class GameHub : Hub<IGameHub>
    {
        private readonly AnswerChoiceDtoMapper _answerChoiceDtoMapper;
        private readonly QuestionInfoDtoMapper _questionInfoDtoMapper;
        private readonly ResultInfoDtoMapper _resultInfoDtoMapper;
        private readonly ConnectionService _connectionService;
        private readonly AnswerService _answerService;
        private readonly GroupService _groupService;
        private readonly GameService _gameService;
        private readonly MatchService _matchService;
        private readonly PlayerService _playerService;
        private readonly ResultService _resultService;
        private readonly UserService _userService;
        private readonly QuestionService _questionService;

        public GameHub(AnswerChoiceDtoMapper answerChoiceMapper, QuestionInfoDtoMapper questionInfoDtoMapper,
            ResultInfoDtoMapper resultInfoDtoMapper, AnswerService answerService, ConnectionService connectionService,
            GroupService groupService, GameService gameService, MatchService matchService, PlayerService playerService,
            ResultService resultService, UserService userService, QuestionService questionService)
        {
            _answerChoiceDtoMapper = answerChoiceMapper;
            _questionInfoDtoMapper = questionInfoDtoMapper;
            _resultInfoDtoMapper = resultInfoDtoMapper;
            _answerService = answerService;
            _connectionService = connectionService;
            _groupService = groupService;
            _gameService = gameService;
            _matchService = matchService;
            _playerService = playerService;
            _resultService = resultService;
            _userService = userService;
            _questionService = questionService;
        }

        public override async Task OnConnectedAsync()
        {
            var user = await _userService.GetOne(Context.GetUsername());

            if (!IsUserHostBot(user))
            {
                var game = await _gameService.GetOne(Context.GetGameId(), GameStatus.WAITING_FOR_PLAYERS_TO_CONNECT);
                await JoinGame(game, user);
            }
            else
            {
                await LaunchGame(Context.GetGameId());
            }
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var game = await _gameService.GetOne(Context.GetGameId());
            var user = await _userService.GetOne(Context.GetUsername());
            
            await base.OnDisconnectedAsync(exception);
        }

        public async Task LaunchGame(int gameId)
        {
            var game = await _gameService.GetOne(gameId, GameStatus.PLAYERS_CONNECTED);

            await PrepareMatch(game);
            await PrepareResults(game);
            await PrepareQuestions(game);
            await ReadyForGame(game);
            await GameInProgress(game);
            await ManageGame(game);
        }

        [HubMethodName("AnswerQuestion")]
        public async Task AnswerQuestion(int selectedAnswerIndex)
        {
            var game = await _gameService.GetOne(Context.GetGameId(), GameStatus.GAME_IN_PROGRESS);
            var match = await _matchService.GetOneLatest(game.Id.Value);

            var user = await _userService.GetOne(Context.GetUsername());
            var player = await _playerService.GetQuietly(game.Id.Value, user.Id);
            var result = await _resultService.GetOne(game.Id.Value, match.Id.Value, player.Id.Value);

            var question = await _questionService.GetOneCurrentQuietly(game.Id.Value, match.Id.Value);
            var existingAnswer = await _answerService.GetOneQuietly(game.Id.Value, match.Id.Value, player.Id.Value, question.Id.Value);

            if (question != null && question.RemainingTime > 0 && existingAnswer == null)
            {
                var answer = new Answer
                {
                    SelectedAnswer = question.AnswerChoices[selectedAnswerIndex],
                    SelectedAnswerIndex = selectedAnswerIndex,
                    Game = game,
                    GameId = game.Id.Value,
                    Match = match,
                    MatchId = match.Id.Value,
                    Player = player,
                    PlayerId = player.Id.Value,
                    Question = question,
                    QuestionId = question.Id.Value
                };

                if (selectedAnswerIndex == question.CorrectAnswerIndex)
                {
                    var score = question.RemainingTime * question.ScoreMultiplier;
                    answer.IsCorrectAnswer = true;
                    answer.Score = score;
                    result.TotalScore += score;
                }
                else
                {
                    answer.IsCorrectAnswer = false;
                }

                await _answerService.Save(answer);
                await _resultService.Save(result);

                await SendAnswerChoiceToAllUsers(game, match, question);
                await SendResultInfoToAllUsers(game, match, question);
            }
        }

        private async Task JoinGame(Game game, User user)
        {
            await SendLoadingMessageToUser(user, "Joining game...");

            var player = await _playerService.GetQuietly(game.Id.Value, user.Id);

            if (player == null)
            {
                throw new SignalToAnswerException("Invalid Game");
            }
            else if (player.PlayerStatus != PlayerStatus.WAITING_TO_JOIN)
            {
                throw new SignalToAnswerException("You cannot join again to same Game!");
            }

            await _playerService.ChangePlayerStatus(player, PlayerStatus.JOINED_GAME);
        }

        private async Task PrepareMatch(Game game)
        {
            var status = await _gameService.GetStatus(game.Id.Value);

            if (await CheckIfAllUsersAreInGame(game) && status == GameStatus.PLAYERS_CONNECTED)
            {
                await SendLoadingMessageToAllUsers(game, "Creating match...");
                await _matchService.CreateMatch(game);
                await _gameService.ChangeStatus(game, GameStatus.MATCH_CREATED);
            }
            else
            {
                await CancelGame(game);
            }
        }

        private async Task PrepareResults(Game game)
        {
            var status = await _gameService.GetStatus(game.Id.Value);

            if (await CheckIfAllUsersAreInGame(game) && status == GameStatus.MATCH_CREATED)
            {
                await SendLoadingMessageToAllUsers(game, "Creating result board...");
                var match = await _matchService.GetOneLatest(game.Id.Value);
                await _resultService.CreateResults(game, match);
                await _gameService.ChangeStatus(game, GameStatus.RESULT_BOARD_CREATED);
            }
            else
            {
                await CancelGame(game);
            }
        }


        private async Task PrepareQuestions(Game game)
        {
            var status = await _gameService.GetStatus(game.Id.Value);

            if (await CheckIfAllUsersAreInGame(game) && status == GameStatus.RESULT_BOARD_CREATED)
            {
                await SendLoadingMessageToAllUsers(game, "Fetching questions...");
                var match = await _matchService.GetOneLatest(game.Id.Value);
                await _questionService.CreateQuestions(game, match);
                await _gameService.ChangeStatus(game, GameStatus.QUESTIONS_CREATED);
            }
            else
            {
                await CancelGame(game);
            }
        }

        private async Task ReadyForGame(Game game)
        {
            var status = await _gameService.GetStatus(game.Id.Value);

            if (await CheckIfAllUsersAreInGame(game) && status == GameStatus.QUESTIONS_CREATED)
            {
                await SendLoadingMessageToAllUsers(game, "Ready for game...");
                await _gameService.ChangeStatus(game, GameStatus.READY_FOR_GAME);
            }
            else
            {
                await CancelGame(game);
            }
        }

        private async Task GameInProgress(Game game)
        {
            var status = await _gameService.GetStatus(game.Id.Value);

            if (await CheckIfAllUsersAreInGame(game) && status == GameStatus.READY_FOR_GAME)
            {
                await _gameService.ChangeStatus(game, GameStatus.GAME_IN_PROGRESS);
                await ManageGame(game);
            }
            else
            {
                await CancelGame(game);
            }
        }

        private async Task ManageGame(Game game)
        {
            var match = await _matchService.GetOneLatest(game.Id.Value);
            var question = await _questionService.GetOneCurrentQuietly(game.Id.Value, match.Id.Value);

            await HideLoadingToAllUsers(game);

            do
            {
                await SendResultInfoToAllUsers(game, match);
                await SendQuestionInfoToAllUsers(game, question, false);
                await SendAnswerChoiceToAllUsers(game, question);

                for (int time = 10; time >= 0; time--)
                {
                    if (!await CheckIfAllUsersAreInGame(game))
                    {
                        break;
                    }

                    await SendTimerInfoToAllUsers(game, time);
                    await _questionService.ChangeRemainingTime(question, time);
                    await Task.Delay(1000);
                }

                await _questionService.MarkAsComplete(question);

                if (!await CheckIfAllUsersAreInGame(game))
                {
                    break;
                }

                await SendAnswerChoiceToAllUsers(game, match, question);
                await SendQuestionInfoToAllUsers(game, question, true);

                await Task.Delay(5000);
                question = await _questionService.GetOneCurrentQuietly(game.Id.Value, match.Id.Value);
            } while (question != null);

            var results = await _resultService.GetAll(game.Id.Value, match.Id.Value);
        }

        private async Task SendLoadingMessageToUser(User user, string message)
        {
            var connection = await _connectionService.GetOne(user.Id);
            await Clients.User(connection.UserIdentifier).ReceiveLoadingMessage(message);
        }

        private async Task SendLoadingMessageToAllUsers(Game game, string message)
        {
            var connections = await GetInGameConnections(game);
            connections.ForEach(async c => await Clients.User(c.UserIdentifier).ReceiveLoadingMessage(message));
        }

        private async Task HideLoadingToAllUsers(Game game)
        {
            var connections = await GetInGameConnections(game);
            connections.ForEach(async c => await Clients.User(c.UserIdentifier).ReceiveHideLoading());
        }

        private async Task SendResultInfoToAllUsers(Game game, Match match)
        {
            var results = await _resultService.GetAll(game.Id.Value, match.Id.Value);
            var resultInfoDtos = _resultInfoDtoMapper.Map(results);

            var connections = await GetInGameConnections(game);
            connections.ForEach(async c => await Clients.User(c.UserIdentifier).ReceiveResultInfo(resultInfoDtos));
        }

        private async Task SendResultInfoToAllUsers(Game game, Match match, Question question)
        {
            var results = await _resultService.GetAll(game.Id.Value, match.Id.Value);
            var resultInfoDtos = _resultInfoDtoMapper.Map(results, question);

            var connections = await GetInGameConnections(game);
            connections.ForEach(async c => await Clients.User(c.UserIdentifier).ReceiveResultInfo(resultInfoDtos));
        }

        private async Task SendQuestionInfoToAllUsers(Game game, Question question, bool showCorrectAnswer)
        {
            var questionDisplayDto = _questionInfoDtoMapper.Map(question, game.QuestionsCount);

            if (showCorrectAnswer)
            {
                questionDisplayDto.CorrectAnswer = question.CorrectAnswer;
            }

            var connections = await GetInGameConnections(game);
            connections.ForEach(async c => await Clients.User(c.UserIdentifier).ReceiveQuestionInfo(questionDisplayDto));
        }

        private async Task SendAnswerChoiceToAllUsers(Game game, Question question)
        {
            var answerChoiceDtos = _answerChoiceDtoMapper.Map(question);

            var connections = await GetInGameConnections(game);
            connections.ForEach(async c => await Clients.User(c.UserIdentifier).ReceiveAnswerChoice(answerChoiceDtos));
        }

        private async Task SendAnswerChoiceToAllUsers(Game game, Match match, Question question)
        {
            var connections = await GetInGameConnections(game);

            connections.ForEach(async (c) =>
            {
                var player = await _playerService.GetQuietly(game.Id.Value, c.UserId);

                var answer = await _answerService.GetOneQuietly(game.Id.Value, match.Id.Value, player.Id.Value, question.Id.Value);
                var otherIncorrectAnswers = (await _answerService.GetAllIncorrect(game.Id.Value, match.Id.Value, player.Id.Value, question.Id.Value))
                    .DistinctBy(a => a.SelectedAnswerIndex);

                var answerChoiceDtos = _answerChoiceDtoMapper.Map(question);

                foreach (AnswerChoiceDto dto in answerChoiceDtos)
                {
                    if (otherIncorrectAnswers.Any(a => a.SelectedAnswer.Equals(dto.Answer)))
                    {
                        dto.Status = AnswerStatus.INCORRECT;
                        dto.IsDisabled = true;
                    }
                    else if (answer != null)
                    {
                        dto.IsDisabled = true;

                        if (answer.SelectedAnswer.Equals(dto.Answer))
                        {
                            if (answer.IsCorrectAnswer)
                            {
                                dto.Status = AnswerStatus.CORRECT;
                            }
                            else
                            {
                                dto.Status = AnswerStatus.INCORRECT;
                            }
                        }
                    }

                    if (question.RemainingTime <= 0)
                    {
                        dto.IsDisabled = true;
                    }
                }

                await Clients.User(c.UserIdentifier).ReceiveAnswerChoice(answerChoiceDtos);
            });
        }

        private async Task SendTimerInfoToAllUsers(Game game, int time)
        {
            var connections = await GetInGameConnections(game);
            connections.ForEach(async c => await Clients.User(c.UserIdentifier).ReceiveTimerInfo(time));
        }

        private bool IsUserHostBot(User user)
        {
            return user.UserRole.Role.Name.Equals(RoleType.HOST_BOT);
        }

        private async Task<List<Connection>> GetInGameConnections(Game game)
        {
            var inGame = await _groupService.GetOneInGame(game.Id.Value, game.GameType);
            
            return await _connectionService.GetAll(inGame.Id.Value);
        }

        private async Task<bool> CheckIfAllUsersAreInGame(Game game)
        {
            var connections = await GetInGameConnections(game);
            var status = await _gameService.GetStatus(game.Id.Value);

            return connections.Count == game.MaxPlayerCount && status != GameStatus.PLAYER_DISCONNECTED;
        }

        private async Task CancelGame(Game game)
        {
            var match = await _matchService.GetOneLatest(game.Id.Value);
            var finishedMatches = await _matchService.GetAllFinished(game.Id.Value);

            if (match != null)
            {
                await _matchService.Deactivate(match);
            }

            if (finishedMatches.Count < 1)
            {
                await _gameService.Deactivate(game);
            }

            Context.Abort();

            var connections = await GetInGameConnections(game);
            connections.ForEach(c => Clients.User(c.UserIdentifier).ReceiveGameCancelled("User disconnected from in-game lobby!"));
        }
    }
}
