using SignalToAnswer.Entities;
using SignalToAnswer.Integrations.TriviaApi.Models;
using SignalToAnswer.Integrations.TriviaApi.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SignalToAnswer.Integrations.TriviaApi.Mappers
{
    public class TARequestMapper
    {
        private readonly TAQuestionCategoryRepository _categoryRepository;
        private readonly TAQuestionDifficultyRepository _difficultyRepository;

        public TARequestMapper(TAQuestionCategoryRepository categoryRepository, TAQuestionDifficultyRepository difficultyRepository)
        {
            _categoryRepository = categoryRepository;
            _difficultyRepository = difficultyRepository;
        }

        public async Task<TARequest> Map(Game game)
        {
            var request = new TARequest
            {
                Categories = await GetCategories(game.QuestionCategories),
                Difficulty = await GetDifficulty(game.QuestionDifficulty),
                Limit = GetLimit(game.QuestionsCount)
            };
           
            return request;
        }

        private async Task<string> GetCategories(List<int> questionCategories)
        {
            if (questionCategories.Count == 0)
            {
                return "";
            }

            var categories = new List<string>();

            foreach (var id in questionCategories)
            {
                var category = await _categoryRepository.FindOneById(id);
                categories.Add(category.Param);
            }

            return "categories=" + string.Join(',', categories);
        }

        private async Task<string> GetDifficulty(int? questionDifficulty)
        {
            if (questionDifficulty == 0)
            {
                return "";
            }
            
            var difficulty = await _difficultyRepository.FindOneById(questionDifficulty.Value);

            return "difficulty=" + difficulty.Param;
        }

        private string GetLimit(int? questionCount)
        {
            return "limit=" + questionCount;
        }
    }
}
