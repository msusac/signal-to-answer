using Microsoft.EntityFrameworkCore;
using SignalToAnswer.Data;
using SignalToAnswer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalToAnswer.Repositories
{
    public class GameRepository
    {
        private readonly DataContext _dataContext;

        public GameRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<List<Game>> FindAllByGameTypeAndGameStatus(int gameType, int gameStatus)
        {
            return await _dataContext.Games.Where(g => g.GameType.Equals(gameType) && g.GameStatus.Equals(gameStatus) && g.Active.Equals(true))
                .ToListAsync();
        }

        public async Task<List<int?>> FindAllIdByGameTypeAndGameStatus(int gameType, int gameStatus)
        {
            return await _dataContext.Games.Where(g => g.GameType.Equals(gameType) && g.GameStatus.Equals(gameStatus) && g.Active.Equals(true))
                .Select(g => g.Id).ToListAsync();
        } 

        public async Task<Game> FindOneFirstByGameTypeAndGameStatusOrderedByCreatedAtAsc(int gameType, int gameStatus)
        {
            return await _dataContext.Games.Where(g => g.GameType.Equals(gameType) && g.GameStatus.Equals(gameStatus) && g.Active.Equals(true))
                .OrderBy(g => g.CreatedAt)
                .FirstOrDefaultAsync();
        }

        public async Task<Game> FindOneByIdAndGameStatus(int id, int gameStatus)
        {
            return await _dataContext.Games
                .SingleOrDefaultAsync(g => g.Id.Equals(id) && g.GameStatus.Equals(gameStatus) && g.Active.Equals(true));
        }

        public async Task<Game> Save(Game game)
        {
            if (game.Id == null)
            {
                await _dataContext.Games.AddAsync(game);
            }
            else
            {
                game.UpdatedAt = DateTime.Now;

                _dataContext.Games.Update(game);
            }

            await _dataContext.SaveChangesAsync();

            return game;
        }
    }
}
