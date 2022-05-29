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
        public async Task<Game> CreateGame()
        {
            var game = new Game
            {
                MaxPlayerCount = 2,
                GameStatus = GameStatus.CREATED,
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
                GameStatus = GameStatus.CREATED,
                GameType = gameType,
                QuestionCategories = questionCategories,
                QuestionsCount = questionsCount,
                QuestionDifficulty = difficultyType
            };

            if (gameType.Equals(GameType.SOLO))
            {
                game.MaxPlayerCount = 1;
            }
            else
            {
                game.MaxPlayerCount = 2;
            }

            await _gameRepository.Save(game);

            return game;
        }

        public async Task<List<Game>> GetAll(int gameType, int gameStatus)
        {
            return await _gameRepository.FindAllByGameType_AndGameStatus(gameType, gameStatus);
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

        [Transactional]
        public async Task<Game> Save(Game game)
        {
            return await _gameRepository.Save(game);
        }

        [Transactional]
        public async Task<Game> Deactivate(Game game)
        {
            game.Active = false;
            return await _gameRepository.Save(game);
        }
    }
}
