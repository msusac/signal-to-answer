﻿using SignalToAnswer.Constants;
using SignalToAnswer.Dtos.GameHub;
using SignalToAnswer.Entities;
using SignalToAnswer.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SignalToAnswer.Mappers.Dtos.GameHub
{
    public class ResultInfoDtoMapper
    {
        private readonly AnswerService _answerService;
        private readonly GameService _gameService;
        private readonly PlayerService _playerService;
        private readonly UserService _userService;

        public ResultInfoDtoMapper(AnswerService answerService, GameService gameService, PlayerService playerService, UserService userService)
        {
            _answerService = answerService;
            _gameService = gameService;
            _playerService = playerService;
            _userService = userService;
        }

        public async Task<ResultInfoDto> Map(Result result)
        {
            var player = await _playerService.GetOne(result.PlayerId);
            var user = await _userService.GetOne(player.UserId);

            return new ResultInfoDto(user.UserName, result.TotalScore);
        }

        public async Task<ResultInfoDto> Map(Result result, Question question)
        {
            var player = await _playerService.GetOne(result.PlayerId);
            var user = await _userService.GetOne(player.UserId);
            var answer = await _answerService.GetOneQuietly(player.GameId, question.MatchId, player.Id.Value, question.Id.Value);

            if (answer != null)
            {
                if (answer.IsCorrectAnswer)
                {
                    return new ResultInfoDto(user.UserName, result.TotalScore, AnswerStatus.CORRECT, answer.Score);
                }
                else
                {
                    return new ResultInfoDto(user.UserName, result.TotalScore, AnswerStatus.INCORRECT);
                }
            }

            return new ResultInfoDto(user.UserName, result.TotalScore);
        }

        public List<ResultInfoDto> Map(List<Result> results)
        {
            var dtos = new List<ResultInfoDto>();
            results.ForEach(async r => dtos.Add(await Map(r)));

            return dtos;
        }

        public List<ResultInfoDto> Map(List<Result> results, Question question)
        {
            var dtos = new List<ResultInfoDto>();
            results.ForEach(async r => dtos.Add(await Map(r, question)));

            return dtos;
        }
    }
}