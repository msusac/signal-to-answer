using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Quartz;
using SignalToAnswer.Attributes;
using SignalToAnswer.Constants;
using SignalToAnswer.Entities;
using SignalToAnswer.Hubs;
using SignalToAnswer.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalToAnswer.Jobs
{
    [DisallowConcurrentExecution]
    public class PublicLobbyJob : IJob
    {
        private readonly ILogger<PublicLobbyJob> _logger;
        private readonly IHubContext<PresenceHub> _presenceHubContext;
        private readonly ConnectionService _connectionService;
        private readonly GameService _gameService;
        private readonly GroupService _groupService;
        private readonly PlayerService _playerService;
        private readonly UserService _userService;

        public PublicLobbyJob(ILogger<PublicLobbyJob> logger, IHubContext<PresenceHub> presenceHubContext,
            ConnectionService connectionService, GameService gameService, GroupService groupService,
            PlayerService playerService, UserService userService)
        {
            _logger = logger;
            _presenceHubContext = presenceHubContext;
            _connectionService = connectionService;
            _gameService = gameService;
            _groupService = groupService;
            _playerService = playerService;
            _userService = userService;
        }

        [Transactional]
        public async Task Execute(IJobExecutionContext context)
        {
            var publicLobby = await _groupService.GetOneUnique(GroupType.PUBLIC_LOBBY);

            List<Connection> players;

            do
            {
                players = await _connectionService.GetTwoRandom(publicLobby.Id.Value);

                if (players.Count == 2)
                {
                    _logger.LogInformation("Creating a public game for users: {UserIds}", string.Join(", ", players.Select(p => p.UserId).ToList()));

                    var game = await _gameService.CreateGame();
                    var group = await _groupService.CreateInGamePublicGroup(game);

                    foreach (var p in players)
                    {
                        var user = await _userService.GetOne(p.UserId);

                        await _playerService.AddPlayerToGame(game, user);
                        await _connectionService.Save(p, group, p.UserIdentifier);

                        await _presenceHubContext.Clients.User(p.UserIdentifier).SendCoreAsync("ReceiveGroupType", new object[] { group.GroupType });

                        _logger.LogInformation("Launching public game {id}!", game.Id.Value);
                        await _presenceHubContext.Clients.User(p.UserIdentifier).SendCoreAsync("ReceivePublicGame", new object[] { game.Id });
                    }

                    await CountUsersInPublicLobby();
                }
            }
            while (players.Count == 2);
        }

        private async Task CountUsersInPublicLobby()
        {
            var group = await _groupService.GetOneUnique(GroupType.PUBLIC_LOBBY);
            var users = await _connectionService.GetAll(group.Id.Value);
            await _presenceHubContext.Clients.All.SendCoreAsync("ReceivePublicLobbyCount", new object[] { users.Count });
        }
    }
}
