using Microsoft.EntityFrameworkCore;
using SignalToAnswer.Data;
using SignalToAnswer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalToAnswer.Repositories
{
    public class ResultRepository
    {
        private readonly DataContext _dataContext;

        public ResultRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<List<Result>> FindAllByGameIdAndMatchId(int gameId, int matchId)
        {
            return await _dataContext.Results.Where(r => r.GameId.Equals(gameId) && r.MatchId.Equals(matchId) && r.Active.Equals(true))
                .AsNoTracking().ToListAsync();
        }

        public async Task<Result> FindOneByGameIdAndMatchIdAndPlayerId(int gameId, int matchId, int playerId)
        {
            return await _dataContext.Results.Where(r => r.GameId.Equals(gameId) && r.MatchId.Equals(matchId) &&
                r.PlayerId.Equals(playerId) && r.Active.Equals(true)).SingleOrDefaultAsync();
        }

        public async Task<Result> Save(Result result)
        {
            if (result.Id == null)
            {
                await _dataContext.Results.AddAsync(result);
            }
            else
            {
                result.UpdatedAt = DateTime.Now;

                _dataContext.Results.Update(result);
            }

            await _dataContext.SaveChangesAsync();

            return result;
        }
    }
}
