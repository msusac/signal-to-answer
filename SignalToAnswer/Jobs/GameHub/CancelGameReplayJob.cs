using Microsoft.AspNetCore.SignalR;
using Quartz;
using SignalToAnswer.Constants;
using SignalToAnswer.Hubs;
using SignalToAnswer.Services;
using System;
using System.Threading.Tasks;

namespace SignalToAnswer.Jobs
{
    [DisallowConcurrentExecution]
    public class CancelGameReplayJob : IJob
    {
        private readonly IHubContext<GameHub, IGameHub> _gameHubContext;
        private readonly ConnectionService _connectionService;
        private readonly GameService _gameService;
        private readonly GroupService _groupService;

        public CancelGameReplayJob(IHubContext<GameHub, IGameHub> gameHubContext, ConnectionService connectionService, GameService gameService, GroupService groupService)
        {
            _gameHubContext = gameHubContext;
            _connectionService = connectionService;
            _gameService = gameService;
            _groupService = groupService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var gameIds = await _gameService.GetAllId(GameStatus.PLAYERS_WANT_TO_REPLAY);

            gameIds.ForEach(async id =>
            {
                var game = await _gameService.GetOne(id.Value);
                var inGame = await _groupService.GetOne(id.Value);

                var connections = await _connectionService.GetAll(inGame.Id.Value);

                if (DateTime.Now.Subtract(game.UpdatedAt).Minutes >= 1 && game.GameStatus == GameStatus.PLAYERS_WANT_TO_REPLAY && connections.Count < game.MaxPlayerCount)
                {
                    await _gameService.ChangeStatus(game, GameStatus.REPLAY_CANCELLED);

                    await _gameHubContext.Clients.Group(inGame.GroupName).ReceiveGameReplayCancelled("Not enough users for replaying game!");
                }
            });
        }
    }
}
