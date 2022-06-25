using SignalToAnswer.Attributes;
using SignalToAnswer.Constants;
using SignalToAnswer.Entities;
using SignalToAnswer.Exceptions;
using SignalToAnswer.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SignalToAnswer.Services
{
    public class GameService
    {
        private readonly GameRepository _gameRepository;

        public GameService(GameRepository gameRepository)
        {
            _gameRepository = gameRepository;

        }

        [Transactional]
        public async Task<Game> ChangeStatus(Game game, int gameStatus)
        {
            game.GameStatus = gameStatus;
            return await _gameRepository.Save(game);
        }

        [Transactional]
        public async Task<Game> CreateGame()
        {
            var game = new Game
            {
                MaxPlayerCount = 2,
                GameStatus = GameStatus.WAITING_FOR_PLAYERS_TO_CONNECT,
                GameType = GameType.PUBLIC,
                QuestionsCount = 15,
                QuestionDifficulty = QuestionDifficulty.DEFAULT,
                QuestionCategories = new List<int>()
            };

            await _gameRepository.Save(game);

            return game;
        }

        [Transactional]
        public async Task<Game> CreateGame(List<int> questionCategories, int questionsCount, int gameType, int difficultyType)
        {
            var game = new Game
            {       
                GameType = gameType,
                QuestionCategories = questionCategories,
                QuestionsCount = questionsCount,
                QuestionDifficulty = difficultyType
            };

            if (gameType.Equals(GameType.SOLO))
            {
                game.GameStatus = GameStatus.WAITING_FOR_PLAYERS_TO_CONNECT;
                game.MaxPlayerCount = 1;
            }
            else
            {
                game.GameStatus = GameStatus.WAITING_FOR_PLAYERS_TO_ACCEPT_INVITE;
                game.MaxPlayerCount = 2;
            }

            await _gameRepository.Save(game);

            return game;
        }

        public async Task<List<Game>> GetAll(int gameType, int gameStatus)
        {
            return await _gameRepository.FindAllByGameTypeAndGameStatus(gameType, gameStatus);
        }

        public async Task<List<int?>> GetAllId(int gameStatus)
        {
            return await _gameRepository.FindAllIdByGameStatus(gameStatus);
        }

        public async Task<List<int?>> GetAllId(int gameType, int gameStatus)
        {
            return await _gameRepository.FindAllIdByGameTypeAndGameStatus(gameType, gameStatus);
        }

        public async Task<Game> GetOne(int gameId)
        {
            var game = await _gameRepository.FindOneById(gameId);

            if (game == null)
            {
                throw new SignalToAnswerException("Selected game does not exist!");
            }

            return game;
        }

        public async Task<Game> GetOne(int gameId, int gameStatus)
        {
            var game = await _gameRepository.FindOneByIdAndGameStatus(gameId, gameStatus);

            if (game == null)
            {
                throw new SignalToAnswerException("Selected game does not exist!");
            }

            return game;
        }

        public async Task<Game> GetOneQuietly(int gameId, int gameStatus)
        {
            return await _gameRepository.FindOneByIdAndGameStatus(gameId, gameStatus);
        }

        public async Task<int> GetStatusNoTracking(int gameId)
        {
            var game = await _gameRepository.FindOneByIdNoTracking(gameId);

            if (game == null)
            {
                throw new SignalToAnswerException("Selected game does not exist!");
            }

            return game.GameStatus;
        }

        [Transactional]
        public async Task<Game> Save(Game game)
        {
            return await _gameRepository.Save(game);
        }

        [Transactional]
        public async Task Deactivate(Game game)
        {
            game.Active = false;
            await _gameRepository.Save(game);
        }
    }
}
