using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using SignalToAnswer.Attributes;
using SignalToAnswer.Constants;
using SignalToAnswer.Dtos;
using SignalToAnswer.Entities;
using SignalToAnswer.Extensions;
using SignalToAnswer.Mappers.Dtos;
using SignalToAnswer.Services;
using System.Threading.Tasks;

namespace SignalToAnswer.Hubs
{
    public interface IPresenceHub
    {
        public Task ReceiveGroupType(int groupTypeId);

        public Task ReceivePublicLobbyCount(int count);

        public Task ReceiveGame(int gameId);

        public Task ReceivePrivateGameInvite(PrivateGameInviteDto dto);

        public Task ReceivePrivateGameLoadingMessage(string message);

        public Task ReceivePrivateGameCancelled(string message);

        public Task ReceiveWinLossRatio(WinLossRatioDto dto);
    }

    [Authorize]
    public class PresenceHub : Hub<IPresenceHub>
    {
        private readonly WinLossRatioDtoMapper _winLossRatioDtoMapper;
        private readonly ConnectionService _connectionService;
        private readonly GroupService _groupService;
        private readonly UserService _userService;

        public PresenceHub(WinLossRatioDtoMapper winLossRatioDtoMapper, ConnectionService connectionService, GroupService groupService, UserService userService)
        {
            _winLossRatioDtoMapper = winLossRatioDtoMapper;
            _connectionService = connectionService;
            _groupService = groupService;
            _userService = userService;
        }

        public override async Task OnConnectedAsync()
        {
            await ChangeGroupUnique(GroupType.MAIN_LOBBY);
        }

        [Transactional]
        public async Task ChangeGroupUnique(int groupTypeId)
        {
            var group = await _groupService.GetOneUnique(groupTypeId);
            var user = await _userService.GetOne(Context.GetUsername());
            var connection = await _connectionService.GetOne(user.Id);

            await _connectionService.Save(connection, group, Context.GetUserIdentifier());
            await Clients.User(Context.GetUserIdentifier()).ReceiveGroupType(groupTypeId);
            await CountUsersInPublicLobby();
            await RetrieveWinLossRatio(user, connection);
        }

        private async Task CountUsersInPublicLobby()
        {
            var group = await _groupService.GetOneUnique(GroupType.PUBLIC_LOBBY);
            var users = await _connectionService.GetAll(group.Id.Value);
            await Clients.All.ReceivePublicLobbyCount(users.Count);
        }

        private async Task RetrieveWinLossRatio(User user, Connection connection)
        {
            var dto = await _winLossRatioDtoMapper.Map(user);
            await Clients.User(connection.UserIdentifier).ReceiveWinLossRatio(dto);
        }
    }
}
