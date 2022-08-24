using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using SignalToAnswer.Attributes;
using SignalToAnswer.Constants;
using SignalToAnswer.Dtos;
using SignalToAnswer.Entities;
using SignalToAnswer.Exceptions;
using SignalToAnswer.Extensions;
using SignalToAnswer.Mappers.Dtos;
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

        public Task ReceiveEndGameInfo(EndGameInfoDto dto);

        public Task ReceiveGameReplayCancelled(string message);

        public Task ReceiveShowReplayOption(bool show);

        public Task ReceiveGameReplayInvite(string fromUser);

        public Task ReceiveClearGameData();
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
                var game = await _gameService.GetOne(Context.GetGameId(), 
                    GameStatus.WAITING_FOR_PLAYERS_TO_CONNECT);
                var group = await _groupService.GetOneInGame(game.Id.Value, game.GameType);

                await Groups.AddToGroupAsync(Context.ConnectionId, group.GroupName);

                await JoinGame(game, user);
            }
            else
            {
                await LaunchGame(Context.GetGameId());
            }
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var user = await _userService.GetOne(Context.GetUsername());

            if (!IsUserHostBot(user))
            {
                var status = await _gameService.GetStatusNoTrackingActiveExcluded(Context.GetGameId());
                var player = await _playerService.GetOneActiveExcluded(Context.GetGameId(), user.Id);

                var game = await _gameService.GetOneActiveExcluded(Context.GetGameId());
                var group = await _groupService.GetOneInGame(game.Id.Value, game.GameType);

                await Groups.RemoveFromGroupAsync(Context.ConnectionId, group.GroupName);

                if (status == GameStatus.GAME_IN_PROGRESS)
                {
                    var match = await _matchService.GetOneLatest(game.Id.Value);
                    var question = await _questionService.GetOneCurrentQuietly(game.Id.Value, match.Id.Value);

                    if (question != null)
                    {
                        await _playerService.ChangePlayerStatus(player, PlayerStatus.DISCONNECTED_DURING_GAME);
                        await _gameService.ChangeStatus(game, GameStatus.PLAYER_DISCONNECTED_DURING_GAME);
                    }
                    else
                    {
                        await _playerService.ChangePlayerStatus(player, PlayerStatus.DISCONNECTED);
                        await _gameService.ChangeStatus(game, GameStatus.PLAYER_DISCONNECTED);
                    }
                }
                else
                {
                    await _playerService.ChangePlayerStatus(player, GameStatus.PLAYER_DISCONNECTED);

                    if (status == GameStatus.WAITING_FOR_PLAYERS_TO_CONNECT)
                    {
                        await CancelGame(game, "User disconnected from game!");
                    }
                    else if (status == GameStatus.PLAYERS_WANT_TO_REPLAY)
                    {
                        await CancelGameReplay(game, "User disconnected from game!");
                    }
                    else if (status != GameStatus.CANCELLED || status != GameStatus.REPLAY_CANCELLED)
                    {
                        await _gameService.ChangeStatus(game, GameStatus.PLAYER_DISCONNECTED);
                    }
                }

                await SendShowReplayOptionToUsers(game, false);
            }

            await base.OnDisconnectedAsync(exception);

        }

        public async Task LaunchGame(int gameId)
        {
            var game = await _gameService.GetOne(gameId, new List<int>() { GameStatus.PLAYERS_CONNECTED, GameStatus.REPLAYING });

            await ClearGameData(game);
            await PrepareMatch(game);
            await PrepareResults(game);
            await PrepareQuestions(game);
            await ReadyForGame(game);
            await GameInProgress(game);
            await EndGame(game);
        }

        [HubMethodName("AnswerQuestion")]
        public async Task AnswerQuestion(int selectedAnswerIndex)
        {
            var game = await _gameService.GetOne(Context.GetGameId(), GameStatus.GAME_IN_PROGRESS);
            var match = await _matchService.GetOneLatest(game.Id.Value);

            var user = await _userService.GetOne(Context.GetUsername());
            var player = await _playerService.GetOneQuietly(game.Id.Value, user.Id);
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

        [HubMethodName("RequestGameReplay")]
        public async Task RequestGameReplay()
        {
            var game = await _gameService.GetOne(Context.GetGameId());
            var connections = await GetInGameConnections(game);

            if (game.GameStatus != GameStatus.GAME_END || game.GameStatus == GameStatus.REPLAY_CANCELLED || connections.Count < game.MaxPlayerCount)
            {
                throw new SignalToAnswerException("Game Replay Request rejected!");
            }

            var user = await _userService.GetOne(Context.GetUsername());
            var player = await _playerService.GetOneQuietly(game.Id.Value, user.Id);

            await _playerService.ChangeReplayStatus(player, ReplayStatus.WANTS_TO_REPLAY);
            await _gameService.ChangeStatus(game, GameStatus.PLAYERS_WANT_TO_REPLAY);

            await SendLoadingMessageToUser(user, "Sending game replay request...");
            await SendShowReplayOptionToUsers(game, false);

            if (game.GameType != GameType.SOLO)
            {
                await SendGameReplayInviteToUsers(game, user);
            }
        }

        [HubMethodName("ReplyGameReplayInvite")]
        public async Task ReplyToGameReplayInvite(int replyId)
        {
            var game = await _gameService.GetOne(Context.GetGameId());
            var user = await _userService.GetOne(Context.GetUsername());

            if (game.GameStatus != GameStatus.PLAYERS_WANT_TO_REPLAY)
            {
                throw new SignalToAnswerException("Reply to Game Replay invite rejected!");
            }

            var player = await _playerService.GetOneQuietly(game.Id.Value, user.Id);

            if (replyId == ReplayStatus.WANTS_TO_REPLAY)
            {
                await _playerService.ChangeReplayStatus(player, ReplayStatus.WANTS_TO_REPLAY);
                await SendLoadingMessageToUser(user, "Joining to game replay");
            }
            else if (replyId == ReplayStatus.DOES_NOT_WANT_TO_REPLAY)
            {
                await _playerService.ChangeReplayStatus(player, ReplayStatus.DOES_NOT_WANT_TO_REPLAY);
                await CancelGameReplay(game, "User rejected invite to replay game!");
            }
        }

        private async Task JoinGame(Game game, User user)
        {
            await SendLoadingMessageToUser(user, "Joining game...");

            var player = await _playerService.GetOneQuietly(game.Id.Value, user.Id);

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

        private async Task ClearGameData(Game game)
        {
            await SendClearGameDataToUsers(game);
        }

        private async Task PrepareMatch(Game game)
        {
            if (await CheckIfAllUsersAreInGame(game))
            {
                await SendLoadingMessageToAllUsers(game, "Creating match...");
                await _matchService.CreateMatch(game);
                await Task.Delay(550);
            }
            else
            {
                await CancelGame(game, "User disconnected from in-game lobby!");
            }
        }

        private async Task PrepareResults(Game game)
        {
            if (await CheckIfAllUsersAreInGame(game))
            {
                await SendLoadingMessageToAllUsers(game, "Creating result board...");
                var match = await _matchService.GetOneLatest(game.Id.Value);
                await _resultService.CreateResults(game, match);
                await Task.Delay(550);
            }
            else
            {
                await CancelGame(game, "User disconnected from in-game lobby!");
            }
        }


        private async Task PrepareQuestions(Game game)
        {
            if (await CheckIfAllUsersAreInGame(game))
            {
                await SendLoadingMessageToAllUsers(game, "Fetching questions...");
                var match = await _matchService.GetOneLatest(game.Id.Value);
                await _questionService.CreateQuestions(game, match);
                await Task.Delay(550);
            }
            else
            {
                await CancelGame(game, "User disconnected from in-game lobby!");
            }
        }

        private async Task ReadyForGame(Game game)
        {
            if (await CheckIfAllUsersAreInGame(game))
            {
                await SendLoadingMessageToAllUsers(game, "Ready for game...");
                await Task.Delay(550);
            }
            else
            {
                await CancelGame(game, "User disconnected from in-game lobby!");
            }
        }

        private async Task GameInProgress(Game game)
        {
            if (await CheckIfAllUsersAreInGame(game))
            {
                await _gameService.ChangeStatus(game, GameStatus.GAME_IN_PROGRESS);
                await ManageGame(game);
            }
            else
            {
                await CancelGame(game, "User disconnected from in-game lobby!");
            }
        }

        private async Task ManageGame(Game game)
        {
            var match = await _matchService.GetOneLatest(game.Id.Value);
            var question = await _questionService.GetOneCurrentQuietly(game.Id.Value, match.Id.Value);

            await HideLoadingToAllUsers(game);

            do
            {
                await SendResultInfoToAllUsers(game, match, false);
                await SendQuestionInfoToAllUsers(game, question, false);
                await SendAnswerChoiceToAllUsers(game, question);

                for (int time = 10; time >= 0; time--)
                {
                    if ((!await CheckIfAllUsersAreInGame(game)) || (await CheckIfAllUsersAnswered(game, match, question)))
                    {
                        break;
                    }

                    await SendTimerInfoToAllUsers(game, time);
                    await _questionService.ChangeRemainingTime(question, time);
                    await Task.Delay(925);
                }

                await _questionService.MarkAsComplete(question);

                if (!await CheckIfAllUsersAreInGame(game))
                {
                    break;
                }

                await SendAnswerChoiceToAllUsers(game, match, question);
                await SendQuestionInfoToAllUsers(game, question, true);

                question = await _questionService.GetOneCurrentQuietly(game.Id.Value, match.Id.Value);

                await Task.Delay(4000);
            } while (question != null);
        }

        public async Task EndGame(Game game)
        {
            var status = await _gameService.GetStatusNoTrackingActiveExcluded(game.Id.Value);

            if (status != GameStatus.CANCELLED)
            {
                var match = await _matchService.GetOneLatest(game.Id.Value);

                if (game.GameType != GameType.SOLO)
                {
                    if (status == GameStatus.PLAYER_DISCONNECTED_DURING_GAME)
                    {
                        await AddLossToPlayerDisconnectedInGame(game, match);
                    }
                    else
                    {
                        await DetermineWinner(game, match);
                    }
                }

                await _matchService.MarkAsComplete(match);
                await _gameService.ChangeStatus(game, GameStatus.GAME_END);

                await SendEndGameInfoToUsers(game);
                await SendResultInfoToAllUsers(game, match, true);

                if (await CheckIfAllUsersAreInGame(game))
                {
                    await SendShowReplayOptionToUsers(game, true);
                }
            }

            Context.Abort();
        }

        private async Task SendLoadingMessageToUser(User user, string message)
        {
            var connection = await _connectionService.GetOne(user.Id);
            await Clients.User(connection.UserIdentifier).ReceiveLoadingMessage(message);
        }

        private async Task SendLoadingMessageToAllUsers(Game game, string message)
        {
            var group = await _groupService.GetOneInGame(game.Id.Value, game.GameType);
            await Clients.Group(group.GroupName).ReceiveLoadingMessage(message);
        }

        private async Task HideLoadingToAllUsers(Game game)
        {
            var group = await _groupService.GetOneInGame(game.Id.Value, game.GameType);
            await Clients.Group(group.GroupName).ReceiveHideLoading();
        }

        private async Task SendResultInfoToAllUsers(Game game, Match match, bool endGame)
        {
            var results = await _resultService.GetAllNoTracking(game.Id.Value, match.Id.Value);

            var resultInfoDtos = _resultInfoDtoMapper.Map(results, endGame);

            var group = await _groupService.GetOneInGame(game.Id.Value, game.GameType);
            await Clients.Group(group.GroupName).ReceiveResultInfo(resultInfoDtos);
        }

        private async Task SendResultInfoToAllUsers(Game game, Match match, Question question)
        {
            var results = await _resultService.GetAllNoTracking(game.Id.Value, match.Id.Value);
            var resultInfoDtos = _resultInfoDtoMapper.Map(results, question);

            var group = await _groupService.GetOneInGame(game.Id.Value, game.GameType);
            await Clients.Group(group.GroupName).ReceiveResultInfo(resultInfoDtos);
        }

        private async Task SendQuestionInfoToAllUsers(Game game, Question question, bool showCorrectAnswer)
        {
            var questionDisplayDto = _questionInfoDtoMapper.Map(question, game.QuestionsCount, showCorrectAnswer);

            var group = await _groupService.GetOneInGame(game.Id.Value, game.GameType);
            await Clients.Group(group.GroupName).ReceiveQuestionInfo(questionDisplayDto);
        }

        private async Task SendAnswerChoiceToAllUsers(Game game, Question question)
        {
            var answerChoiceDtos = _answerChoiceDtoMapper.Map(question);

            var group = await _groupService.GetOneInGame(game.Id.Value, game.GameType);
            await Clients.Group(group.GroupName).ReceiveAnswerChoice(answerChoiceDtos);
        }

        private async Task SendAnswerChoiceToAllUsers(Game game, Match match, Question question)
        {
            var connections = await GetInGameConnections(game);

            connections.ForEach(async (c) =>
            {
                var player = await _playerService.GetOneQuietly(game.Id.Value, c.UserId);

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
            var group = await _groupService.GetOneInGame(game.Id.Value, game.GameType);
            await Clients.Group(group.GroupName).ReceiveTimerInfo(time);
        }

        private async Task SendEndGameInfoToUsers(Game game)
        {
            var group = await _groupService.GetOneInGame(game.Id.Value, game.GameType);
            await Clients.Group(group.GroupName).ReceiveEndGameInfo(new EndGameInfoDto(game.GameType));
        }

        private async Task SendShowReplayOptionToUsers(Game game, bool showReplayOption)
        {
            var group = await _groupService.GetOneInGame(game.Id.Value, game.GameType);
            await Clients.Group(group.GroupName).ReceiveShowReplayOption(showReplayOption);
        }

        private async Task SendGameReplayInviteToUsers(Game game, User fromUser)
        {
            var group = await _groupService.GetOneInGame(game.Id.Value, game.GameType);
            await Clients.OthersInGroup(group.GroupName).ReceiveGameReplayInvite(fromUser.UserName);
        }

        private async Task SendClearGameDataToUsers(Game game)
        {
            var group = await _groupService.GetOneInGame(game.Id.Value, game.GameType);
            await Clients.Group(group.GroupName).ReceiveClearGameData();
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
            var status = await _gameService.GetStatusNoTrackingActiveExcluded(game.Id.Value);

            return connections.Count == game.MaxPlayerCount && (status != GameStatus.PLAYER_DISCONNECTED || status != GameStatus.PLAYER_DISCONNECTED_DURING_GAME || status != GameStatus.CANCELLED || status != GameStatus.REPLAY_CANCELLED);
        }

        private async Task<bool> CheckIfAllUsersAnswered(Game game, Match match, Question question)
        {
            var answers = await _answerService.GetAllNoTracking(game.Id.Value, match.Id.Value, question.Id.Value);

            return answers.Count == game.MaxPlayerCount;
        }

        private async Task CancelGame(Game game, string message)
        {
            var match = await _matchService.GetOneLatest(game.Id.Value);
            var finishedMatches = await _matchService.GetAllFinished(game.Id.Value);

            await _gameService.ChangeStatus(game, GameStatus.CANCELLED);

            if (match != null)
            {
                await _matchService.Deactivate(match);

                var results = await _resultService.GetAll(game.Id.Value, match.Id.Value);

                if (results.Count > 0)
                {
                    results.ForEach(async r => await _resultService.Deactivate(r));
                }

                var questions = await _questionService.GetAll(game.Id.Value, match.Id.Value);

                if (questions.Count > 0)
                {
                    questions.ForEach(async q => await _questionService.Deactivate(q));
                }
            }

            if (finishedMatches.Count < 1)
            {
                await _gameService.Deactivate(game);

                var players = await _playerService.GetAll(game.Id.Value);

                if (players.Count > 0)
                {
                    players.ForEach(async p => await _playerService.Deactivate(p));
                }
            }

            var group = await _groupService.GetOneInGame(game.Id.Value, game.GameType);
            await Clients.Group(group.GroupName).ReceiveGameCancelled(message);
        }

        private async Task CancelGameReplay(Game game, string message)
        {
            await _gameService.ChangeStatus(game, GameStatus.REPLAY_CANCELLED);

            var group = await _groupService.GetOneInGame(game.Id.Value, game.GameType);
            await Clients.OthersInGroup(group.GroupName).ReceiveGameReplayCancelled(message);
        }

        private async Task AddLossToPlayerDisconnectedInGame(Game game, Match match)
        {
            var players = await _playerService.GetAll(game.Id.Value, PlayerStatus.DISCONNECTED_DURING_GAME);
            var player = players.OrderByDescending(p => p.UpdatedAt).FirstOrDefault();

            var result = await _resultService.GetOneNoTracking(game.Id.Value, match.Id.Value, player.Id.Value);

            await SaveResultEnd(result.Id.Value, result.TotalScore, WinnerStatus.LOSS, "Disconnected during game.");
        }

        private async Task DetermineWinner(Game game, Match match)
        {
            var bestResults = (await _resultService.GetAllNoTracking(game.Id.Value, match.Id.Value))
                .GroupBy(r => r.TotalScore)
                .OrderByDescending(r => r.Key)
                .First()
                .ToList();

            if (bestResults.Count > 1)
            {
                bestResults.ForEach(async r => await SaveResultEndGame(r.Id.Value, r.TotalScore, WinnerStatus.DRAW));
            }
            else
            {
                bestResults.ForEach(async r => await SaveResultEndGame(r.Id.Value, r.TotalScore, WinnerStatus.WIN));
            }

            var bestScore = bestResults.First().TotalScore;

            var otherResults = (await _resultService.GetAllNoTracking(game.Id.Value, match.Id.Value))
                .Where(r => !r.TotalScore.Equals(bestScore))
                .ToList();

            otherResults.ForEach(async r => await SaveResultEndGame(r.Id.Value, r.TotalScore, WinnerStatus.LOSS));
        }

        private async Task SaveResultEndGame(int id, int totalScore, int winnerStatus)
        {
            var result = await _resultService.GetOne(id);
            result.TotalScore = totalScore;
            result.WinnerStatus = winnerStatus;
            await _resultService.Save(result);
        }

        private async Task SaveResultEnd(int id, int totalScore, int winnerStatus, string note)
        {
            var result = await _resultService.GetOne(id);
            result.Note = note;
            result.TotalScore = totalScore;
            result.WinnerStatus = winnerStatus;
            await _resultService.Save(result);
        }
    }
}
