﻿using Quartz;
using SignalToAnswer.Constants;
using SignalToAnswer.Hubs.Contexts;
using SignalToAnswer.Services;
using System;
using System.Threading.Tasks;

namespace SignalToAnswer.Jobs
{
    [DisallowConcurrentExecution]
    public class CancelGameReplayJob : IJob
    {
        private readonly GameHubContext _gameHubContext;
        private readonly ConnectionService _connectionService;
        private readonly GameService _gameService;
        private readonly GroupService _groupService;

        public CancelGameReplayJob(GameHubContext gameHubContext, ConnectionService connectionService, GameService gameService, GroupService groupService)
        {
            _gameHubContext = gameHubContext;
            _connectionService = connectionService;
            _gameService = gameService;
            _groupService = groupService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var gameIds = await _gameService.GetAllId(GameStatus.PLAYERS_WANT_TO_REPLAY);

            foreach(var id in gameIds)
            {
                var game = await _gameService.GetOne(id.Value);
                var inGame = await _groupService.GetOne(id.Value);

                var connections = await _connectionService.GetAll(inGame.Id.Value);

                if (DateTime.Now.Subtract(game.UpdatedAt).Minutes >= 1 && game.GameStatus == GameStatus.PLAYERS_WANT_TO_REPLAY && connections.Count < game.MaxPlayerCount)
                {
                    await _gameService.ChangeStatus(game, GameStatus.REPLAY_CANCELLED);
                    await _gameHubContext.SendGameReplayCancelledToGroup(inGame, "Not enough users for replaying game!");
                }
            }
        }
    }
}
