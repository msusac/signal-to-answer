using Microsoft.AspNetCore.SignalR;
using SignalToAnswer.Attributes;
using SignalToAnswer.Constants;
using SignalToAnswer.Dtos;
using SignalToAnswer.Entities;
using SignalToAnswer.Exceptions;
using SignalToAnswer.Form;
using SignalToAnswer.Hubs;
using SignalToAnswer.Services;
using SignalToAnswer.Validators.Form;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SignalToAnswer.Facades.Hubs
{
    public class GameFacade
    {
        private readonly IHubContext<PresenceHub, IPresenceHub> _presenceHubContext;
        private readonly ConnectionService _connectionService;
        private readonly GameService _gameService;
        private readonly GroupService _groupService;
        private readonly PlayerService _playerService;
        private readonly UserService _userService;
        private readonly CreateGameFormValidator _createGameFormValidator;
        private readonly InviteResponseFormValidator _inviteResponseFormValidator;

        public GameFacade(IHubContext<PresenceHub, IPresenceHub> presenceHubContext, ConnectionService connectionService, 
            GameService gameService, GroupService groupService, PlayerService playerService, 
            UserService userService, CreateGameFormValidator createGameFormValidator, 
            InviteResponseFormValidator inviteResponseFormValidator)
        {
            _presenceHubContext = presenceHubContext;
            _connectionService = connectionService;
            _gameService = gameService;
            _groupService = groupService;
            _playerService = playerService;
            _userService = userService;
            _createGameFormValidator = createGameFormValidator;
            _inviteResponseFormValidator = inviteResponseFormValidator;
        }

        [Transactional]
        public async Task<int> CreateSoloGame(CreateGameForm form, string username)
        {
            var user = await _userService.GetOne(username);

            await IsUserInGroupUnique(user, GroupType.SOLO_LOBBY);
            await _createGameFormValidator.Validate(form, GameType.SOLO);

            var game = await _gameService.CreateGame(form.Categories, form.Limit.Value, GameType.SOLO, form.Difficulty.Value);
            var inGameSolo = await _groupService.CreateInGameSoloGroup(game);

            await _playerService.AddPlayerToGame(game, user);

            await ChangeGroup(inGameSolo, user);

            return game.Id.Value;
        }

        [Transactional]
        public async Task CreatePrivateGame(CreateGameForm form, string username)
        {
            var user = await _userService.GetOne(username);

            await IsUserInGroupUnique(user, GroupType.PRIVATE_LOBBY);
            await _createGameFormValidator.Validate(form, GameType.PRIVATE);

            var game = await _gameService.CreateGame(form.Categories, form.Limit.Value, GameType.PRIVATE, form.Difficulty.Value);
            var inviteLobby = await _groupService.CreateInviteLobbyGroup(game);

            var usernames = new List<string> { username };
            form.InviteUsers.ForEach(user => usernames.Add(user));

            var users = new List<User>();
            usernames.ForEach(async u => users.Add(await _userService.GetOne(u)));

            for (int i = 0; i < usernames.Count; i++)
            {
                if (i == 0)
                {
                    await _playerService.AddPlayerToGame(game, users[i], InviteStatus.SENT_INVITE);
                    await ChangeGroup(inviteLobby, users[i]);
                }
                else
                {
                    await _playerService.AddPlayerToGame(game, users[i], InviteStatus.WAITING_TO_RESPOND);
                    await SendPrivateGameInviteToUser(game, inviteLobby, users[i], username);
                }
            }
        }

        [Transactional]
        public async Task ReplyToPrivateGameInvite(InviteReplyForm form, string username)
        {
            var user = await _userService.GetOne(username);

            await IsUserInGroupUnique(user, GroupType.MAIN_LOBBY);
            await _inviteResponseFormValidator.Validate(form, user);

            var game = await _gameService.GetOne(form.GameId.Value, GameStatus.WAITING_FOR_PLAYERS_TO_ACCEPT_INVITE);
            var inviteLobby = await _groupService.GetOne(form.GroupId.Value);
            var player = await _playerService.GetOneQuietly(game.Id.Value, user.Id);

            if (form.IsAccepted.Value)
            {
                await _playerService.ChangeInviteStatus(player, InviteStatus.ACCEPTED);
                await ChangeGroup(inviteLobby, user);
            }
            else
            {
                await _playerService.ChangeInviteStatus(player, InviteStatus.REJECTED);
                await CancelPrivateGame(game, inviteLobby);
            }
        }

        private async Task SendPrivateGameInviteToUser(Game game, Group inviteLobby, User user, string fromUser)
        {
            var connection = await _connectionService.GetOne(user.Id);
            await _presenceHubContext.Clients.User(connection.UserIdentifier).ReceivePrivateGameInvite(new PrivateGameInviteDto(fromUser, game.Id.Value, inviteLobby.Id.Value));
        }

        [Transactional]
        private async Task CancelPrivateGame(Game game, Group inviteLobby)
        {
            await _gameService.ChangeStatus(game, GameStatus.CANCELLED);
            await _gameService.Deactivate(game);

            var connections = await _connectionService.GetAll(inviteLobby.Id.Value);
            var message = "User rejected invite.";

            connections.ForEach(async c =>
            {
                var mainLobby = await _groupService.GetOneUnique(GroupType.MAIN_LOBBY);
                await ChangeGroup(mainLobby, c);
                await _presenceHubContext.Clients.User(c.UserIdentifier).ReceivePrivateGameCancelled(message);
            });
        }

        private async Task ChangeGroup(Group group, Connection connection)
        {
            await _connectionService.Save(connection, group, connection.UserIdentifier);
            await _presenceHubContext.Clients.User(connection.UserIdentifier).ReceiveGroupType(group.GroupType);
        }

        private async Task ChangeGroup(Group group, User user)
        {
            var connection = await _connectionService.GetOne(user.Id);
            await ChangeGroup(group, connection);
        }

        private async Task IsUserInGroupUnique(User user, int groupType)
        {
            var groupUnique = await _groupService.GetOneUnique(groupType);
            var connection = await _connectionService.GetOne(user.Id);

            if (connection.GroupId != groupUnique.Id)
            {
                throw new SignalToAnswerException(string.Format("You must be in group {0} to proceed with action", groupUnique.GroupName));
            }
        }
    }
}
