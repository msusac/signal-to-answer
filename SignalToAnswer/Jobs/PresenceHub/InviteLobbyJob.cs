﻿using Microsoft.Extensions.Logging;
using Quartz;
using SignalToAnswer.Attributes;
using SignalToAnswer.Constants;
using SignalToAnswer.Entities;
using SignalToAnswer.Hubs.Contexts;
using SignalToAnswer.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SignalToAnswer.Jobs
{
    [DisallowConcurrentExecution]
    public class InviteLobbyJob : IJob
    {
        private readonly ILogger<InviteLobbyJob> _logger;
        private readonly PresenceHubContext _presenceHubContext;
        private readonly ConnectionService _connectionService;
        private readonly GameService _gameService;
        private readonly GroupService _groupService;
        private readonly PlayerService _playerService;

        public InviteLobbyJob(ILogger<InviteLobbyJob> logger, PresenceHubContext presenceHubContext,
            ConnectionService connectionService, GameService gameService, GroupService groupService,
            PlayerService playerService)
        {
            _logger = logger;
            _presenceHubContext = presenceHubContext;
            _connectionService = connectionService;
            _gameService = gameService;
            _groupService = groupService;
            _playerService = playerService;
        }

        [Transactional]
        public async Task Execute(IJobExecutionContext context)
        {
            var gameIds = await _gameService.GetAllId(GameType.PRIVATE, GameStatus.WAITING_FOR_PLAYERS_TO_ACCEPT_INVITE);

            foreach (var id in gameIds)
            {
                var game = await _gameService.GetOneQuietly(id.Value, GameStatus.WAITING_FOR_PLAYERS_TO_ACCEPT_INVITE);

                if (game != null)
                {
                    var inviteLobby = await _groupService.GetOneInviteLobby(game.Id.Value);
                    var players = await _playerService.GetAll(game.Id.Value);

                    if (!await CheckIfHostIsInInviteLobby(players[0], inviteLobby))
                    {
                        _logger.LogInformation("Cancelling private game {id}: Host left the lobby!", game.Id.Value);
                        await CancelPrivateGame(game, inviteLobby, "Host left the invite lobby!");
                    }
                    else if (!await CheckIfOtherPlayersAreInInviteLobby(players, inviteLobby))
                    {
                        _logger.LogInformation("Cancelling private game {id}: Player left the invite lobby!", game.Id.Value);
                        await CancelPrivateGame(game, inviteLobby, "Player left the invite lobby!");
                    }
                    else if (await CheckIfPrivateGameTimeout(game, inviteLobby))
                    {
                        _logger.LogInformation("Cancelling private game {id}: Not enough players in invite lobby!", game.Id.Value);
                        await CancelPrivateGame(game, inviteLobby, "Not enough players connected to lobby on time!");
                    }
                    else if (await GetPlayersInInviteLobbyCount(game, inviteLobby) == game.MaxPlayerCount)
                    {
                        _logger.LogInformation("Launching private game {id}", game.Id.Value);
                        await LaunchPrivateGame(game, inviteLobby);
                    }
                }
            }
        }

        private async Task<bool> CheckIfHostIsInInviteLobby(Player host, Group inviteLobby)
        {
            var connection = await _connectionService.GetOne(host.UserId);

            if (host.InviteStatus.Value == InviteStatus.SENT_INVITE && connection.GroupId != inviteLobby.Id)
            {
                return false;
            }

            return true;
        }

        private async Task<bool> CheckIfOtherPlayersAreInInviteLobby(List<Player> players, Group inviteLobby)
        {
            var condition = true;

            for (int i = 0; i < players.Count; i++)
            {
                var player = players[i];
                var connection = await _connectionService.GetOne(player.UserId);

                if (player.InviteStatus.Value == InviteStatus.ACCEPTED && connection.GroupId != inviteLobby.Id)
                {
                    condition = false;
                    break;
                }
            }

            return condition;
        }

        private async Task<bool> CheckIfPrivateGameTimeout(Game game, Group inviteLobby)
        {
            return await GetPlayersInInviteLobbyCount(game, inviteLobby) < game.MaxPlayerCount && (DateTime.Now - game.CreatedAt).Seconds > 30;
        }

        private async Task CancelPrivateGame(Game game, Group inviteLobby, string message)
        {
            await _gameService.ChangeStatus(game, GameStatus.CANCELLED);
            await _gameService.Deactivate(game);

            var connections = await _connectionService.GetAll(inviteLobby.Id.Value);

            foreach (var c in connections)
            {
                var mainLobby = await _groupService.GetOneUnique(GroupType.MAIN_LOBBY);
                await _presenceHubContext.ChangeGroup(mainLobby, c);

                await _presenceHubContext.SendPrivateGameCancelledToUser(c, message);
            }
        }

        private async Task LaunchPrivateGame(Game game, Group inviteLobby)
        {
            await _gameService.ChangeStatus(game, GameStatus.WAITING_FOR_PLAYERS_TO_CONNECT);

            var inGamePrivate = await _groupService.CreateInGamePrivateGroup(game);
            var connections = await _connectionService.GetAll(inviteLobby.Id.Value);

            foreach (var c in connections)
            {

                await _presenceHubContext.ChangeGroup(inGamePrivate, c);
                await _presenceHubContext.SendGameToUser(game, c);
            }
        }

        private async Task<int> GetPlayersInInviteLobbyCount(Game game, Group inviteLobby)
        {
            var connections = await _connectionService.GetAll(inviteLobby.Id.Value);
            var message = string.Format("{0} / {1} players connected in invite lobby.\n Launching game when all players are connected.", connections.Count, game.MaxPlayerCount);

            foreach (var c in connections)
            {
                await _presenceHubContext.SendPrivateGameLoadingMessage(c, message);
            }

            return connections.Count;
        }
    }

}