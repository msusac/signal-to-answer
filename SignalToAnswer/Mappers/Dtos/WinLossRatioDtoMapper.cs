using SignalToAnswer.Constants;
using SignalToAnswer.Dtos;
using SignalToAnswer.Entities;
using SignalToAnswer.Services;
using System;
using System.Threading.Tasks;

namespace SignalToAnswer.Mappers.Dtos
{
    public class WinLossRatioDtoMapper
    {
        private readonly GameService _gameService;
        private readonly MatchService _matchService;
        private readonly PlayerService _playerService;
        private readonly ResultService _resultService;

        public WinLossRatioDtoMapper(GameService gameService, MatchService matchService, PlayerService playerService, ResultService resultService)
        {
            _gameService = gameService;
            _matchService = matchService;
            _playerService = playerService;
            _resultService = resultService;
        }

        public async Task<WinLossRatioDto> Map(User user)
        {
            int wins = 0;
            int losses = 0;

            var players = await _playerService.GetAll(user.Id);

            foreach (var p in players)
            {
                var game = await _gameService.GetOneQuietly(p.GameId);

                if (game != null && game.GameType != GameType.SOLO)
                {
                    var matches = await _matchService.GetAllFinished(p.GameId);

                    foreach (var m in matches)
                    {
                        var result = await _resultService.GetOne(p.GameId, m.Id.Value, p.Id.Value);

                        if (result.WinnerStatus == WinnerStatus.WIN)
                        {
                            wins++;
                        }
                        else if (result.WinnerStatus == WinnerStatus.LOSS)
                        {
                            losses++;
                        }
                    }
                }
            }

            decimal ratio = GetWinLossRatio(wins, losses);

            return new WinLossRatioDto(wins, losses, ratio);
        }

        private decimal GetWinLossRatio(int wins, int losses)
        {
            if (wins == 0)
            {
                return 0;
            }
            else if (losses == 0)
            {
                return Math.Round((decimal)wins, 2);
            }
            else
            {
                return Math.Round((decimal) wins / losses, 2);
            }
        }
    }
}
