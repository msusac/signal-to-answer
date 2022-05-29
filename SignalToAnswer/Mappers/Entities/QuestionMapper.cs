using SignalToAnswer.Constants;
using SignalToAnswer.Entities;
using SignalToAnswer.Integrations.TriviaApi.Models;
using SignalToAnswer.Integrations.TriviaApi.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalToAnswer.Mappers.Dtos
{
    public class QuestionMapper
    {
        private readonly TAQuestionCategoryRepository _categoryRepository;
        private readonly TAQuestionDifficultyRepository _difficultyRepository;

        public QuestionMapper(TAQuestionCategoryRepository categoryRepository, TAQuestionDifficultyRepository difficultyRepository)
        {
            _categoryRepository = categoryRepository;
            _difficultyRepository = difficultyRepository;
        }

        public async Task<Question> Map(TAQuestion taQuestion)
        {
            var question = new Question
            {
                Category = await GetCategory(taQuestion.Category),
                CorrectAnswer = GetCorrectAnswer(taQuestion.CorrectAnswer),
                Description = taQuestion.Question,
                Difficulty = await GetDifficulty(taQuestion.Difficulty)
            };

            question.AnswerChoices = GetAnswerChoices(taQuestion.IncorrectAnswers, question.CorrectAnswer);
            question.Attempts = GetAttempts(question.Difficulty);
            question.ScoreMultiplier = GetScoreMultiplier(question.Difficulty);
            
            return question;
        }

        public List<Question> Map(List<TAQuestion> taQuestions)
        {
            List<Question> questionList = new();

            taQuestions.ForEach(async q => questionList.Add(await Map(q)));

            return questionList;
        }

        private List<string> GetAnswerChoices(List<string> incorrectAnswers, string correctAnswer)
        {
            var answers = new List<string>();
            incorrectAnswers.Take(5).ToList().ForEach(q => answers.Add(q.Trim()));
            answers.Add(correctAnswer);

            var rand = new Random();
            return answers.OrderBy(q => rand.Next()).ToList();
        }

        private async Task<int> GetCategory(string category)
        {
            return (await _categoryRepository.FindOneByNameUpper(category)).Id.Value;
        }

        private string GetCorrectAnswer(string correctAnswer)
        {
            return correctAnswer.Trim();
        }

        private async Task<int> GetDifficulty(string difficulty)
        {
            return (await _difficultyRepository.FindOneByNameUpper(difficulty)).Id.Value;
        }

        private int GetAttempts(int difficulty)
        {
            if (difficulty == QuestionDifficulty.EASY)
            {
                return 1;
            }

            return 2;
        }

        private int GetScoreMultiplier(int difficulty)
        {
            if (difficulty == QuestionDifficulty.MEDIUM)
            {
                return 2;
            }
            else if (difficulty < QuestionDifficulty.HARD)
            {
                return 3;
            }
            else
            {
                return 1;
            }
        }
    }
}
