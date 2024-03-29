﻿using Microsoft.EntityFrameworkCore;
using SignalToAnswer.Data;
using SignalToAnswer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalToAnswer.Repositories
{
    public class QuestionRepository
    {
        private readonly DataContext _dataContext;

        public QuestionRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<List<Question>> FindAllByGameIdAndMatchId(int gameId, int matchId)
        {
            return await _dataContext.Questions.Where(q => q.GameId.Equals(gameId) && q.MatchId.Equals(matchId)
                && q.Active.Equals(true)).ToListAsync();
        }
        public async Task<Question> FindOneByGameIdAndMatchIdAndIsOngoingOrderedByRowIdAsc(int gameId, int matchId)
        {
            return await _dataContext.Questions.Where(q => q.GameId.Equals(gameId) && q.MatchId.Equals(matchId)
                && q.IsOngoing.Equals(true) && q.Active.Equals(true))
                .OrderBy(q => q.Row)
                .FirstOrDefaultAsync();
        }

        public async Task<Question> Save(Question question)
        {
            if (question.Id == null)
            {
                await _dataContext.Questions.AddAsync(question);
            }
            else
            {
                question.UpdatedAt = DateTime.Now;

                _dataContext.Questions.Update(question);
            }

            await _dataContext.SaveChangesAsync();

            return question;
        }
    }
}
