using Microsoft.EntityFrameworkCore;
using SignalToAnswer.Data;
using SignalToAnswer.Integrations.TriviaApi.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SignalToAnswer.Integrations.TriviaApi.Repositories
{
    public class TAQuestionCategoryRepository
    {
        private readonly DataContext _dataContext;

        public TAQuestionCategoryRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<IEnumerable<TAQuestionCategory>> FindAll()
        {
            return await _dataContext.TAQuestionCategories.ToListAsync();
        }

        public async Task<TAQuestionCategory> FindOneById(int id)
        {
            return await _dataContext.TAQuestionCategories.SingleOrDefaultAsync(c => c.Id.Equals(id));
        }

        public async Task<TAQuestionCategory> FindOneByName(string name)
        {
            return await _dataContext.TAQuestionCategories.SingleOrDefaultAsync(c => c.Name.Equals(name));
        }
    }
}
