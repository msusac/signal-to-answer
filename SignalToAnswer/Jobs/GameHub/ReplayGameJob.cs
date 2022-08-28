using Microsoft.AspNetCore.Hosting;
using Quartz;
using SignalToAnswer.Constants;
using SignalToAnswer.Hubs.Contexts;
using SignalToAnswer.Services;
using System.Threading.Tasks;

namespace SignalToAnswer.Jobs
{
    [DisallowConcurrentExecution]
    public class ReplayGameJob : IJob
    {
        private readonly GameHubContext _gameHubContext;
        private readonly ConnectionService _connectionService;
        private readonly GameService _gameService;
        private readonly PlayerService _playerService;

        public ReplayGameJob(GameHubContext gameHubContext, ConnectionService connectionService, GameService gameService, PlayerService playerService)
        {
            _gameHubContext = gameHubContext;
            _connectionService = connectionService;
            _gameService = gameService;
            _playerService = playerService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var gameIds = await _gameService.GetAllId(GameStatus.PLAYERS_WANT_TO_REPLAY);

            gameIds.ForEach(async id =>
            {
                var game = await _gameService.GetOneQuietly(id.Value, GameStatus.PLAYERS_WANT_TO_REPLAY);

                if (game != null)
                {
                    var players = await _playerService.GetAll(game.Id.Value, PlayerStatus.JOINED_GAME,
                        ReplayStatus.WANTS_TO_REPLAY);

                    players.ForEach(async p =>
                    {
                        var connection = await _connectionService.GetOne(p.UserId);
                        var message = string.Format("{0} / {1} players waiting for game replay.", 
                            players.Count, game.MaxPlayerCount);

                        await _gameHubContext.SendLoadingMessageToUser(connection, message);
                    });

                    if (players.Count == game.MaxPlayerCount)
                    {
                        players.ForEach(async p => await _playerService.RemoveReplayStatus(p));
                        await _gameService.ChangeStatus(game, GameStatus.REPLAYING);
                        await _gameHubContext.SendHostBotToGame(game);
                    }
                }
            });
        }
    }
}
