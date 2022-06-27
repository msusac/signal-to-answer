using SignalToAnswer.Attributes;
using SignalToAnswer.Constants;
using SignalToAnswer.Entities;
using SignalToAnswer.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SignalToAnswer.Services
{
    public class PlayerService
    {
        private readonly PlayerRepository _playerRepository;

        public PlayerService(PlayerRepository playerRepository)
        {
            _playerRepository = playerRepository;
        }

        [Transactional]
        public async Task<Player> AddPlayerToGame(Game game, User user)
        {
            var player = new Player
            {
                Game = game,
                GameId = game.Id.Value,
                User = user,
                UserId = user.Id,
                PlayerStatus = PlayerStatus.WAITING_TO_JOIN
            };

            await _playerRepository.Save(player);

            return player;
        }

        [Transactional]
        public async Task<Player> AddPlayerToGame(Game game, User user, int inviteStatus)
        {
            var player = new Player
            {
                Game = game,
                GameId = game.Id.Value,
                User = user,
                UserId = user.Id,
                PlayerStatus = PlayerStatus.WAITING_TO_JOIN,
                InviteStatus = inviteStatus
            };

            await _playerRepository.Save(player);

            return player;
        }

        [Transactional]
        public async Task ChangeInviteStatus(Player player, int inviteStatus)
        {
            player.InviteStatus = inviteStatus;
            await _playerRepository.Save(player);
        }

        [Transactional]

        public async Task ChangePlayerStatus(Player player, int playerStatus)
        {
            player.PlayerStatus = playerStatus;
            await _playerRepository.Save(player);
        }

        [Transactional]
        public async Task ChangeReplayStatus(Player player, int? replayStatus)
        {
            player.ReplayStatus = replayStatus;
            await _playerRepository.Save(player);
        }

        public async Task<List<Player>> GetAll(int gameId)
        {
            return await _playerRepository.FindAllByGameId(gameId);
        }

        public async Task<List<Player>> GetAll(int gameId, int playerStatus)
        {
            return await _playerRepository.FindAllByGameIdAndPlayerStatus(gameId, playerStatus);
        }

        public async Task<List<Player>> GetAll(int gameId, int playerStatus, int replayStatus)
        {
            return await _playerRepository.FindAllByGameIdAndPlayerStatusAndReplayStatus(gameId, playerStatus, replayStatus);
        }

        public async Task<Player> GetOne(int id)
        {
            return await _playerRepository.FindOneById(id);
        }

        public async Task<Player> GetQuietly(int gameId, Guid userId)
        {
            return await _playerRepository.FindOneByGameIdAndUserId(gameId, userId);
        }

        public async Task<Player> GetOneActiveExcluded(int gameId, Guid userId)
        {
            return await _playerRepository.FindOneByGameIdAndUserIdActiveExcluded(gameId, userId);
        }

        [Transactional]
        public async Task Deactivate(Player player)
        {
            player.Active = false;
            await _playerRepository.Save(player);
        }
    }
}
