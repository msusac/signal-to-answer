using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Quartz;
using SignalToAnswer.Constants;
using SignalToAnswer.Entities;
using SignalToAnswer.Services;
using System.Threading.Tasks;

namespace SignalToAnswer.Jobs
{
    [DisallowConcurrentExecution]
    public class LaunchGameJob : IJob
    {
        private readonly IWebHostEnvironment _env;
        private readonly GameService _gameService;
        private readonly PlayerService _playerService;
        private readonly TokenService _tokenService;
        private readonly UserService _userService;

        public LaunchGameJob(IWebHostEnvironment env, GameService gameService, PlayerService playerService, TokenService tokenService, UserService userService)
        {
            _env = env;
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
            var token = await _tokenService.GenerateToken(hostBot, RoleType.HOST_BOT);

            var gameUrl = "http://localhost:5000/hub/game-hub";

            if (!_env.IsDevelopment())
            {
                gameUrl = "/api/hub/game-hub";
            }

            var gameHubConnection = new HubConnectionBuilder()
                .WithUrl(gameUrl + "?gameId=" + game.Id.Value, options =>
                {
                    options.AccessTokenProvider = () => Task.FromResult(token);
                })
                .ConfigureLogging(options => options.SetMinimumLevel(LogLevel.Information))
                .Build();

            await gameHubConnection.StartAsync();
        }
    }
}
