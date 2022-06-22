using Microsoft.EntityFrameworkCore;
using SignalToAnswer.Data;
using SignalToAnswer.Integrations.TriviaApi.Entities;
using System.Threading.Tasks;

namespace SignalToAnswer.Integrations.TriviaApi.Repositories
{
    public class TAQuestionDifficultyRepository
    {
        private readonly DataContext _dataContext;

        public TAQuestionDifficultyRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<TAQuestionDifficulty> FindOneById(int id)
        {
            return await _dataContext.TAQuestionDifficulty.SingleOrDefaultAsync(d => d.Id.Equals(id));
        }

        public async Task<TAQuestionDifficulty> FindOneByParamUpper(string param)
        {
            return await _dataContext.TAQuestionDifficulty.SingleOrDefaultAsync(d => d.Param.ToUpper().Equals(param.ToUpper()));
        }
    }
}
