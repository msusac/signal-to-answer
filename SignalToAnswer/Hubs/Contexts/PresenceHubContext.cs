using Microsoft.AspNetCore.SignalR;
using SignalToAnswer.Constants;
using SignalToAnswer.Dtos;
using SignalToAnswer.Entities;
using SignalToAnswer.Services;
using System.Threading.Tasks;

namespace SignalToAnswer.Hubs.Contexts
{
    public class PresenceHubContext
    {
        private readonly IHubContext<PresenceHub, IPresenceHub> _presenceHubContext;
        private readonly GroupService _groupService;
        private readonly ConnectionService _connectionService;

        public PresenceHubContext(IHubContext<PresenceHub, IPresenceHub> presenceHubContext, GroupService groupService, ConnectionService connectionService)
        {
            _presenceHubContext = presenceHubContext;
            _groupService = groupService;
            _connectionService = connectionService;
        }

        public async Task CountUsersInPublicLobby()
        {
            var group = await _groupService.GetOneUnique(GroupType.PUBLIC_LOBBY);
            var users = await _connectionService.GetAll(group.Id.Value);
            await _presenceHubContext.Clients.All.ReceivePublicLobbyCount(users.Count);
        }

        public async Task ChangeGroup(Group group, Connection connection)
        {
            await _connectionService.Save(connection, group, connection.UserIdentifier);
            await _presenceHubContext.Clients.User(connection.UserIdentifier).ReceiveGroupType(group.GroupType);
        }

        public async Task ChangeGroup(Group group, User user)
        {
            var connection = await _connectionService.GetOne(user.Id);
            await ChangeGroup(group, connection);
        }

        public async Task SendGameToUser(Game game, Connection connection)
        {
            await _presenceHubContext.Clients.User(connection.UserIdentifier).ReceiveGame(game.Id.Value);
        }

        public async Task SendGameToUser(Game game, User user)
        {
            var connection = await _connectionService.GetOne(user.Id);
            await SendGameToUser(game, connection);
        }

        public async Task SendPrivateGameCancelledToUser(Connection connection, string message)
        {
            await _presenceHubContext.Clients.User(connection.UserIdentifier).ReceivePrivateGameCancelled(message);
        }

        public async Task SendPrivateGameInviteToUser(Game game, Group inviteLobby, User user, string fromUser)
        {
            var connection = await _connectionService.GetOne(user.Id);
            await _presenceHubContext.Clients.User(connection.UserIdentifier).ReceivePrivateGameInvite(new PrivateGameInviteDto(fromUser, game.Id.Value, inviteLobby.Id.Value));
        }

        public async Task SendPrivateGameLoadingMessage(Connection connection, string message)
        {
            await _presenceHubContext.Clients.User(connection.UserIdentifier).ReceivePrivateGameLoadingMessage(message);
        }
    }
}
