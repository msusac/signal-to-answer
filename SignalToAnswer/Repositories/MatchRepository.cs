using Microsoft.EntityFrameworkCore;
using SignalToAnswer.Data;
using SignalToAnswer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalToAnswer.Repositories
{
    public class MatchRepository
    {
        private readonly DataContext _dataContext;

        public MatchRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<List<Match>> FindAllByGameIdAndOngoingFalse(int gameId)
        {
            return await _dataContext.Matches.Where(m => m.GameId.Equals(gameId) && m.IsOngoing.Equals(false) && m.Active.Equals(true))
                .ToListAsync();
        }

        public async Task<Match> FindOneByGameIdAndIsOngoingTrueOrderedByCreatedAtDesc(int gameId)
        {
            return await _dataContext.Matches.Where(m => m.GameId.Equals(gameId) && m.IsOngoing.Equals(true) && m.Active.Equals(true))
                .OrderByDescending(m => m.CreatedAt)
                .FirstOrDefaultAsync();
        }

        public async Task<Match> Save(Match match)
        {
            if (match.Id == null)
            {
                await _dataContext.Matches.AddAsync(match);
            }
            else
            {
                match.UpdatedAt = DateTime.Now;

                _dataContext.Matches.Update(match);
            }

            await _dataContext.SaveChangesAsync();

            return match;
        }
    }
}
