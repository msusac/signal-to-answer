using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SignalToAnswer.Constants;
using SignalToAnswer.Entities;
using SignalToAnswer.Services;
using System.Threading.Tasks;

namespace SignalToAnswer.Hubs.Contexts
{
    public class GameHubContext
    {
        private readonly IWebHostEnvironment _env;
        private readonly IHubContext<GameHub, IGameHub> _gameHubContext;
        private readonly TokenService _tokenService;
        private readonly UserService _userService;

        public GameHubContext(IWebHostEnvironment env, IHubContext<GameHub, IGameHub> gameHubContext, TokenService tokenService, UserService userService)
        {
            _env = env;
            _gameHubContext = gameHubContext;
            _tokenService = tokenService;
            _userService = userService;
        }

        public async Task SendGameCancelledToGroup(Group group, string message)
        {
            await _gameHubContext.Clients.Group(group.GroupName).ReceiveGameCancelled(message);
        }

        public async Task SendGameReplayCancelledToGroup(Group group, string message)
        {
            await _gameHubContext.Clients.Group(group.GroupName).ReceiveGameReplayCancelled(message);
        }

        public async Task SendLoadingMessageToUser(Connection connection, string message)
        {
            await _gameHubContext.Clients.User(connection.UserIdentifier).ReceiveLoadingMessage(message);
        }

        public async Task SendHostBotToGame(Game game)
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
