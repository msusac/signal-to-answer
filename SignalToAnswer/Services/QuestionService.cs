using SignalToAnswer.Attributes;
using SignalToAnswer.Entities;
using SignalToAnswer.Integrations.TriviaApi.Services;
using SignalToAnswer.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SignalToAnswer.Services
{
    public class QuestionService
    {
        private readonly QuestionRepository _questionRepository;
        private readonly TAService _taService;

        public QuestionService(QuestionRepository questionRepository, TAService taService)
        {
            _questionRepository = questionRepository;
            _taService = taService;
        }

        public async Task ChangeRemainingTime(Question question, int remainingTime)
        {
            question.RemainingTime = remainingTime;
            await _questionRepository.Save(question);
        }

        public async Task<List<Question>> CreateQuestions(Game game, Match match)
        {
            var questions = await _taService.RetrieveQuestions(game, match);
            questions.ForEach(async q => await _questionRepository.Save(q));

            return questions;
        }

        public async Task<List<Question>> GetAll(int gameId, int matchId)
        {
            return await _questionRepository.FindAllByGameIdAndMatchId(gameId, matchId);
        }

        public async Task<Question> GetOneCurrentQuietly(int gameId, int matchId)
        {
            return await _questionRepository.FindOneByGameIdAndMatchIdAndIsOngoingOrderedByRowIdAsc(gameId, matchId);
        }

        [Transactional]
        public async Task MarkAsComplete(Question question)
        {
            question.IsOngoing = false;
            await _questionRepository.Save(question);
        }

        [Transactional]
        public async Task Deactivate(Question question)
        {
            question.Active = false;
            await _questionRepository.Save(question);
        }
    }
}
