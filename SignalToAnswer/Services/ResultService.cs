using SignalToAnswer.Attributes;
using SignalToAnswer.Entities;
using SignalToAnswer.Exceptions;
using SignalToAnswer.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SignalToAnswer.Services
{
    public class ResultService
    {
        private readonly ResultRepository _resultRepository;
        private readonly PlayerService _playerService;

        public ResultService(ResultRepository resultRepository, PlayerService playerService)
        {
            _resultRepository = resultRepository;
            _playerService = playerService;
        }

        [Transactional]
        public async Task<Result> CreateResult(Game game, Match match, Player player)
        {
            var result = new Result
            {
                GameId = game.Id.Value,
                MatchId = match.Id.Value,
                PlayerId = player.Id.Value,
            };

            return await _resultRepository.Save(result);
        }

        [Transactional]
        public async Task<List<Result>> CreateResults(Game game, Match match)
        {
            var players = await _playerService.GetAll(game.Id.Value);

            var results = new List<Result>();

            foreach (var p in players)
            {
                results.Add(await CreateResult(game, match, p));
            }

            return results;
        }

        public async Task<List<Result>> GetAll(int gameId, int matchId)
        {
            return await _resultRepository.FindAllByGameIdAndMatchId(gameId, matchId);
        }

        public async Task<List<Result>> GetAllNoTracking(int gameId, int matchId)
        {
            return await _resultRepository.FindAllByGameIdAndMatchIdNoTracking(gameId, matchId);
        }

        public async Task<Result> GetOne(int id)
        {
            var result = await _resultRepository.FindOneById(id);

            if (result == null)
            {
                throw new SignalToAnswerException("Selected result does not exist!");
            }

            return result;
        }

        public async Task<Result> GetOne(int gameId, int matchId, int playerId)
        {
            var result =  await _resultRepository.FindOneByGameIdAndMatchIdAndPlayerId(gameId, matchId, playerId);

            if (result == null)
            {
                throw new SignalToAnswerException("Selected result does not exist!");
            }

            return result;
        }

        public async Task<Result> GetOneNoTracking(int gameId, int matchId, int playerId)
        {
            var result = await _resultRepository.FindOneByGameIdAndMatchIdAndPlayerIdNoTracking(gameId, matchId, playerId);

            if (result == null)
            {
                throw new SignalToAnswerException("Selected result does not exist!");
            }

            return result;
        }

        [Transactional]
        public async Task<Result> Save(Result result)
        {
            return await _resultRepository.Save(result);
        }

        [Transactional]
        public async Task Deactivate(Result result)
        {
            result.Active = false;
            await _resultRepository.Save(result);
        }
    }
}
