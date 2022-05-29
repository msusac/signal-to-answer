using Microsoft.AspNetCore.SignalR;
using Quartz;
using SignalToAnswer.Attributes;
using SignalToAnswer.Constants;
using SignalToAnswer.Entities;
using SignalToAnswer.Hubs;
using SignalToAnswer.Services;
using System;
using System.Threading.Tasks;

namespace SignalToAnswer.Jobs
{
    public class InviteLobbyJob : IJob
    {
        private readonly IHubContext<PresenceHub> _presenceHubContext;
        private readonly ConnectionService _connectionService;
        private readonly GameService _gameService;
        private readonly GroupService _groupService;

        public InviteLobbyJob(IHubContext<PresenceHub> presenceHubContext, ConnectionService connectionService, 
            GameService gameService,GroupService groupService)
        {
            _presenceHubContext = presenceHubContext;
            _connectionService = connectionService;
            _gameService = gameService;
            _groupService = groupService;
        }

        [Transactional]
        public async Task Execute(IJobExecutionContext context)
        {
            var games = await _gameService.GetAll(GameType.PRIVATE, GameStatus.CREATED);

            foreach (var g in games)
            {
                var inviteLobby = await _groupService.GetOneInviteLobby(g.Id.Value);
                var connections = await _connectionService.GetAll(inviteLobby.Id.Value);

                if (connections.Count < g.MaxPlayerCount && (DateTime.Now - g.CreatedAt).Seconds > 35)
                {
                    await _gameService.Deactivate(g);

                    connections.ForEach(async c =>
                    {
                        var mainLobby = await _groupService.GetOneUnique(GroupType.MAIN_LOBBY);
                        await ChangeGroup(mainLobby, c);

                        await _presenceHubContext.Clients.User(c.UserIdentifier).SendCoreAsync("ReceivePrivateGameInviteTimeout", new object[] { g.Id.Value });
                    });
                }
            }
        }

        private async Task ChangeGroup(Group group, Connection connection)
        {
            await _connectionService.Save(connection, group, connection.UserIdentifier);
            await _presenceHubContext.Clients.User(connection.UserIdentifier).SendCoreAsync("ReceiveGroupType", new object[] { group.GroupType });
        }
    }
}
