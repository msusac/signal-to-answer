using SignalToAnswer.Attributes;
using SignalToAnswer.Entities;
using SignalToAnswer.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SignalToAnswer.Services
{
    public class MatchService
    {
        private readonly MatchRepository _matchRepository;

        public MatchService(MatchRepository matchRepository)
        {
            _matchRepository = matchRepository;
        }

        [Transactional]
        public async Task<Match> CreateMatch(Game game)
        {
            var match = new Match
            {
                Game = game,
                GameId = game.Id.Value
            };

            return await _matchRepository.Save(match);
        }

        public async Task<List<Match>> GetAllFinished(int gameId)
        {
            return await _matchRepository.FindAllByGameIdAndOngoingFalse(gameId);
        }

        public async Task<Match> GetOneLatest(int gameId)
        {
            return await _matchRepository.FindOneByGameIdAndIsOngoingTrueOrderedByCreatedAtDesc(gameId);
        }

        public async Task<Match> Deactivate(Match match)
        {
            match.Active = false;
            return await _matchRepository.Save(match);
        }
    }
}
