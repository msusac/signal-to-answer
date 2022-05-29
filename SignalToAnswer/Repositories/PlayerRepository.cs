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

        public async Task<List<Player>> FindAllByGame_Id(int gameId)
        {
            return await _dataContext.Players
                .Where(p => p.GameId.Equals(gameId) && p.Active.Equals(true))
                .ToListAsync();
        }

        public async Task<Player> FindOneByGame_IdAnd_User_Id(int gameId, Guid userId)
        {
            return await _dataContext.Players
                .SingleOrDefaultAsync(p => p.GameId.Equals(gameId) && p.UserId.Equals(userId) && p.Active.Equals(true));
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
