using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using SignalToAnswer.Constants;
using SignalToAnswer.Extensions;
using SignalToAnswer.Services;
using System;
using System.Threading.Tasks;

namespace SignalToAnswer.Hubs
{
    [Authorize]
    public class GameHub : Hub
    {
        private readonly GameService _gameService;
        private readonly UserService _userService;

        public GameHub(GameService gameService, UserService userService)
        {
            _gameService = gameService;
            _userService = userService;
        }

        public override async Task OnConnectedAsync()
        {
            var game = await _gameService.GetOne(Context.GetGameId(), GameStatus.WAITING_FOR_PLAYERS_TO_CONNECT);
            var user = await _userService.GetOne(Context.GetUsername());
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }
    }
}
