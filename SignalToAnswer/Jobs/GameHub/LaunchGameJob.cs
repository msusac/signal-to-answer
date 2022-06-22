using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using Quartz;
using SignalToAnswer.Constants;
using SignalToAnswer.Entities;
using SignalToAnswer.Hubs;
using SignalToAnswer.Services;
using System.Threading.Tasks;

namespace SignalToAnswer.Jobs
{
    [DisallowConcurrentExecution]
    public class LaunchGameJob : IJob
    {
        private readonly GameService _gameService;
        private readonly PlayerService _playerService;
        private readonly TokenService _tokenService;
        private readonly UserService _userService;

        public LaunchGameJob(GameService gameService, PlayerService playerService, TokenService tokenService, UserService userService)
        {
            _gameService = gameService;
            _playerService = playerService;
            _tokenService = tokenService;
            _userService = userService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var gameIds = await _gameService.GetAllId(GameStatus.WAITING_FOR_PLAYERS_TO_CONNECT);

            gameIds.ForEach(async id =>
            {
                var game = await _gameService.GetOneQuietly(id.Value, GameStatus.WAITING_FOR_PLAYERS_TO_CONNECT);

                if (game != null)
                {
                    var players = await _playerService.GetAll(game.Id.Value, PlayerStatus.JOINED_GAME);

                    if (players.Count == game.MaxPlayerCount)
                    {
                        await _gameService.ChangeStatus(game, GameStatus.PLAYERS_CONNECTED);
                        await SendLaunchGameToHostBot(game);
                    }
                }
            });
        }

        private async Task SendLaunchGameToHostBot(Game game)
        {
            var hostBot = await _userService.CreateHostBot();
            var token = await _tokenService.GenerateToken(hostBot);

            var gameHubConnection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5000/hub/game-hub?gameId=" + game.Id.Value, options =>
                {
                    options.AccessTokenProvider = () => Task.FromResult(token);
                })
                .ConfigureLogging(options => options.SetMinimumLevel(LogLevel.Information))
                .Build();

            await gameHubConnection.StartAsync();
        }
    }
}
