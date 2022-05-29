using Microsoft.EntityFrameworkCore;
using SignalToAnswer.Data;
using SignalToAnswer.Entities;
using System;
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
