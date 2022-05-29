using SignalToAnswer.Attributes;
using SignalToAnswer.Constants;
using SignalToAnswer.Entities;
using SignalToAnswer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<List<Player>> GetAll(int gameId)
        {
            return await _playerRepository.FindAllByGame_Id(gameId);
        }

        public async Task<Player> GetQuietly(int gameId, Guid userId)
        {
            return await _playerRepository.FindOneByGame_IdAnd_User_Id(gameId, userId);
        }

        public async Task<int> GetCountQuietly(int id)
        {
            return (await _playerRepository.FindAllByGame_Id(id)).Count();
        }
    }
}
