using Quartz;
using SignalToAnswer.Constants;
using SignalToAnswer.Hubs.Contexts;
using SignalToAnswer.Services;
using System.Threading.Tasks;

namespace SignalToAnswer.Jobs
{
    [DisallowConcurrentExecution]
    public class LaunchGameJob : IJob
    {
        private readonly GameHubContext _gameHubContext;
        private readonly GameService _gameService;
        private readonly PlayerService _playerService;

        public LaunchGameJob(GameHubContext gameHubContext, GameService gameService, PlayerService playerService)
        {
            _gameHubContext = gameHubContext;
            _gameService = gameService;
            _playerService = playerService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var gameIds = await _gameService.GetAllId(GameStatus.WAITING_FOR_PLAYERS_TO_CONNECT);

            foreach (var id in gameIds)
            {
                var game = await _gameService.GetOneQuietly(id.Value, GameStatus.WAITING_FOR_PLAYERS_TO_CONNECT);

                if (game != null)
                {
                    var players = await _playerService.GetAll(game.Id.Value, PlayerStatus.JOINED_GAME);

                    if (players.Count == game.MaxPlayerCount)
                    {
                        await _gameService.ChangeStatus(game, GameStatus.PLAYERS_CONNECTED);
                        await _gameHubContext.SendHostBotToGame(game);
                    }
                }
            }
        }
    }
}
