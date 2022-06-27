using SignalToAnswer.Constants;
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
        private readonly PlayerService _playerService;
        private readonly UserService _userService;

        public ResultInfoDtoMapper(AnswerService answerService, PlayerService playerService, UserService userService)
        {
            _answerService = answerService;
            _playerService = playerService;
            _userService = userService;
        }

        public async Task<ResultInfoDto> Map(Result result, bool endGame)
        {
            var player = await _playerService.GetOne(result.PlayerId);
            var user = await _userService.GetOne(player.UserId);

            if (endGame)
            {
                return new ResultInfoDto(user.UserName, result.TotalScore, result.WinnerStatus, result.Note);
            }
    
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

        public List<ResultInfoDto> Map(List<Result> results, bool endGame)
        {
            var dtos = new List<ResultInfoDto>();
            results.ForEach(async r => dtos.Add(await Map(r, endGame)));

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
