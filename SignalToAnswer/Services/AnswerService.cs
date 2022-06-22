using SignalToAnswer.Attributes;
using SignalToAnswer.Entities;
using SignalToAnswer.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SignalToAnswer.Services
{
    public class AnswerService
    {
        private readonly AnswerRepository _answerRepository;

        public AnswerService(AnswerRepository answerRepository)
        {
            _answerRepository = answerRepository;
        }

        public async Task<List<Answer>> GetAllIncorrect(int gameId, int matchId, int playerId, int questionId) 
        {
            return await _answerRepository.FindAllByGameIdAndMatchIdAndPlayerIdNotAndQuestionIdAndIsCorrectAnswerFalse(gameId, matchId, playerId, questionId);
        }

        public async Task<Answer> GetOneQuietly(int gameId, int matchId, int playerId, int questionId)
        {
            return await _answerRepository.FindOneByGameIdAndMatchIdAndPlayerIdAndQuestionId(gameId, matchId, playerId, questionId);
        }

        [Transactional]
        public async Task<Answer> Save(Answer answer)
        {
            return await _answerRepository.Save(answer);
        }
    }
}
