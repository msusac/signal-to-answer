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
        private readonly IHubContext<PresenceHub, IPresenceHub> _presenceHubContext;
        private readonly ConnectionService _connectionService;
        private readonly GameService _gameService;
        private readonly GroupService _groupService;
        private readonly PlayerService _playerService;
        private readonly UserService _userService;

        public PublicLobbyJob(ILogger<PublicLobbyJob> logger, IHubContext<PresenceHub, IPresenceHub> presenceHubContext,
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

            List<Connection> connections;

            do
            {
                connections = await _connectionService.GetTwoRandom(publicLobby.Id.Value);

                if (connections.Count == 2)
                {
                    _logger.LogInformation("Creating a public game for users: {UserIds}", string.Join(", ", connections.Select(p => p.UserId).ToList()));

                    var game = await _gameService.CreateGame();
                    var group = await _groupService.CreateInGamePublicGroup(game);

                    connections.ForEach(async c => await SendPublicGameToUser(c, game, group));
            
                    await CountUsersInPublicLobby();
                }
            }
            while (connections.Count == 2);
        }

        private async Task SendPublicGameToUser(Connection connection, Game game, Group group)
        {
            var user = await _userService.GetOne(connection.UserId);

            await _playerService.AddPlayerToGame(game, user);
            await _connectionService.Save(connection, group, connection.UserIdentifier);

            await _presenceHubContext.Clients.User(connection.UserIdentifier).ReceiveGroupType(group.GroupType);

            _logger.LogInformation("Launching public game {id}!", game.Id.Value);
            await _presenceHubContext.Clients.User(connection.UserIdentifier).ReceivePublicGame(game.Id.Value);
        }

        private async Task CountUsersInPublicLobby()
        {
            var group = await _groupService.GetOneUnique(GroupType.PUBLIC_LOBBY);
            var users = await _connectionService.GetAll(group.Id.Value);
            await _presenceHubContext.Clients.All.ReceivePublicLobbyCount(users.Count);
        }
    }
}
