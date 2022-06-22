using Microsoft.EntityFrameworkCore;
using SignalToAnswer.Data;
using SignalToAnswer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalToAnswer.Repositories
{
    public class AnswerRepository
    {
        private readonly DataContext _dataContext;

        public AnswerRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<List<Answer>> FindAllByGameIdAndMatchIdAndPlayerIdNotAndQuestionIdAndIsCorrectAnswerFalse(int gameId, int matchId, int playerId, int questionId)
        {
            return await _dataContext.Answers.Where(a => a.GameId.Equals(gameId) && a.MatchId.Equals(matchId) &&
                !a.PlayerId.Equals(playerId) && a.QuestionId.Equals(questionId) && a.IsCorrectAnswer.Equals(false) 
                && a.Active.Equals(true)).ToListAsync();
        }

        public async Task<Answer> FindOneByGameIdAndMatchIdAndPlayerIdAndQuestionId(int gameId, int matchId, int playerId, int questionId)
        {
            return await _dataContext.Answers.Where(a => a.GameId.Equals(gameId) && a.MatchId.Equals(matchId) &&
                a.PlayerId.Equals(playerId) && a.QuestionId.Equals(questionId) && a.Active.Equals(true)).SingleOrDefaultAsync(); 
        }

        public async Task<Answer> Save(Answer answer)
        {
            if (answer.Id == null)
            {
                await _dataContext.Answers.AddAsync(answer);
            }
            else
            {
                answer.UpdatedAt = DateTime.Now;

                _dataContext.Answers.Update(answer);
            }

            await _dataContext.SaveChangesAsync();

            return answer;
        }
    }
}
