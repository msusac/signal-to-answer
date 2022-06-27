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

        public async Task<Question> Map(TAQuestion taQuestion, Game game, Match match, int row)
        {
            var question = new Question
            {
                Row = row,
                CorrectAnswer = GetCorrectAnswer(taQuestion.CorrectAnswer),
                Description = taQuestion.Question,
                Difficulty = await GetDifficulty(taQuestion.Difficulty),
                Category = await GetCategory(taQuestion.Category),
                Game = game,
                GameId = game.Id.Value,
                Match = match,
                MatchId = match.Id.Value
            };

            question.AnswerChoices = GetAnswerChoices(taQuestion.IncorrectAnswers, question.CorrectAnswer);
            question.ScoreMultiplier = GetScoreMultiplier(question.Difficulty);
            question.CorrectAnswerIndex = GetCorrectAnswerIndex(question.CorrectAnswer, question.AnswerChoices);
            
            return question;
        }

        public List<Question> Map(List<TAQuestion> taQuestions, Game game, Match match)
        {
            var row = 0;
            var questionList = new List<Question>();

            taQuestions.ForEach(async q => {
                row++;
                questionList.Add(await Map(q, game, match, row));
            });

            return questionList;
        }

        private List<string> GetAnswerChoices(List<string> incorrectAnswers, string correctAnswer)
        {
            var answers = new List<string>();
            incorrectAnswers.ToList().ForEach(q => answers.Add(q.Trim()));
            answers.Add(correctAnswer);

            var rand = new Random();
            return answers.OrderBy(q => rand.Next()).ToList();
        }

        private async Task<int> GetCategory(string category)
        {
            var cat = await _categoryRepository.FindOneByName(category);
            return cat.Id.Value;
        }

        private string GetCorrectAnswer(string correctAnswer)
        {
            return correctAnswer.Trim();
        }

        private int GetCorrectAnswerIndex(string correctAnswer, List<string> answers)
        {
            return answers.FindIndex(a => a.Equals(correctAnswer));
        }

        private async Task<int> GetDifficulty(string difficulty)
        {
            var item = await _difficultyRepository.FindOneByParamUpper(difficulty);

            if (item == null)
            {
                return QuestionDifficulty.MEDIUM;
            }

            return item.Id.Value;
        }

        private int GetScoreMultiplier(int difficulty)
        {
            if (difficulty == QuestionDifficulty.MEDIUM)
            {
                return 2;
            }
            else if (difficulty == QuestionDifficulty.HARD)
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
