using Microsoft.EntityFrameworkCore;
using SignalToAnswer.Data;
using SignalToAnswer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalToAnswer.Repositories
{
    public class PlayerRepository
    {
        private readonly DataContext _dataContext;

        public PlayerRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<List<Player>> FindAllByGameId(int gameId)
        {
            return await _dataContext.Players
                .Where(p => p.GameId.Equals(gameId) && p.Active.Equals(true))
                .ToListAsync();
        }

        public async Task<List<Player>> FindAllByUserId(Guid userId)
        {
            return await _dataContext.Players
                .Where(p => p.UserId.Equals(userId) && p.Active.Equals(true))
                .ToListAsync();
        }

        public async Task<List<Player>> FindAllByGameIdAndPlayerStatus(int gameId, int playerStatus)
        {
            return await _dataContext.Players
               .Where(p => p.GameId.Equals(gameId) && p.PlayerStatus.Equals(playerStatus) && p.Active.Equals(true))
               .ToListAsync();
        }

        public async Task<List<Player>> FindAllByGameIdAndPlayerStatusAndReplayStatus(int gameId, int playerStatus, int replayStatus)
        {
            return await _dataContext.Players
               .Where(p => p.GameId.Equals(gameId) && p.PlayerStatus.Equals(playerStatus) && p.ReplayStatus.Equals(replayStatus) && p.Active.Equals(true))
               .ToListAsync();
        }

        public async Task<Player> FindOneById(int id)
        {
            return await _dataContext.Players.SingleOrDefaultAsync(p => p.Id.Equals(id) && p.Active.Equals(true));
        }

        public async Task<Player> FindOneByGameIdAndUserId(int gameId, Guid userId)
        {
            return await _dataContext.Players
                .SingleOrDefaultAsync(p => p.GameId.Equals(gameId) && p.UserId.Equals(userId) && p.Active.Equals(true));
        }

        public async Task<Player> FindOneByGameIdAndUserIdActiveExcluded(int gameId, Guid userId)
        {
            return await _dataContext.Players.SingleOrDefaultAsync(p => p.GameId.Equals(gameId) && p.UserId.Equals(userId));
        }

        public async Task<Player> Save(Player player)
        {
            if (player.Id == null)
            {
                await _dataContext.Players.AddAsync(player);
            }
            else
            {
                player.UpdatedAt = DateTime.Now;

                _dataContext.Players.Update(player);
            }

            await _dataContext.SaveChangesAsync();

            return player;
        }
    }
}
