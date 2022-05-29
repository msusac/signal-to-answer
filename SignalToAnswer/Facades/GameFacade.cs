using Microsoft.AspNetCore.SignalR;
using SignalToAnswer.Attributes;
using SignalToAnswer.Constants;
using SignalToAnswer.Entities;
using SignalToAnswer.Form;
using SignalToAnswer.Hubs;
using SignalToAnswer.Services;
using SignalToAnswer.Validators.Form;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SignalToAnswer.Facades
{
    public class GameFacade
    {
        private readonly IHubContext<PresenceHub> _presenceHubContext;
        private readonly ConnectionService _connectionService;
        private readonly GameService _gameService;
        private readonly GroupService _groupService;
        private readonly PlayerService _playerService;
        private readonly UserService _userService;
        private readonly CreateGameFormValidator _createGameFormValidator;
        private readonly InviteResponseFormValidator _inviteResponseFormValidator;

        public GameFacade(IHubContext<PresenceHub> presenceHubContext, ConnectionService connectionService, GameService gameService,
            GroupService groupService, PlayerService playerService, UserService userService, CreateGameFormValidator createGameFormValidator,
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
            await _createGameFormValidator.Validate(form, GameType.SOLO);

            var game = await _gameService.CreateGame(form.Categories, form.Limit.Value, GameType.SOLO, form.Difficulty.Value);
            var inGameSolo = await _groupService.CreateInGameSoloGroup(game);
            var user = await _userService.GetOne(username);

            await _playerService.AddPlayerToGame(game, user);

            await ChangeGroup(inGameSolo, user);

            return game.Id.Value;
        }

        [Transactional]
        public async Task CreatePrivateGame(CreateGameForm form, string username)
        {
            await _createGameFormValidator.Validate(form, GameType.PRIVATE);

            var game = await _gameService.CreateGame(form.Categories, form.Limit.Value, GameType.PRIVATE, form.Difficulty.Value);
            var inviteLobby = await _groupService.CreateInviteLobbyGroup(game);

            List<string> usernames = new() { username };
            form.InviteUsers.ForEach(user => usernames.Add(user));

            List<User> users = new();
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

                    var connection = await _connectionService.GetOne(users[i].Id);
                    await _presenceHubContext.Clients.User(connection.UserIdentifier).SendCoreAsync("ReceivePrivateGameInvite", new object[] { username, game.Id.Value, inviteLobby.Id.Value });
                }
            }
        }

        [Transactional]
        public async Task RespondToPrivateGameInvite(InviteResponseForm form, string username)
        {
            var user = await _userService.GetOne(username);

            await _inviteResponseFormValidator.Validate(form, user);

            var game = await _gameService.GetOne(form.GameId.Value, GameStatus.CREATED);
            var inviteLobby = await _groupService.GetOne(form.GroupId.Value);
            var player = await _playerService.GetQuietly(game.Id.Value, user.Id);

            if (form.IsAccepted.Value)
            {
                await _playerService.ChangeInviteStatus(player, InviteStatus.ACCEPTED);
                await ChangeGroup(inviteLobby, user);

                var connections = await _connectionService.GetAll(inviteLobby.Id.Value);

                if (connections.Count == game.MaxPlayerCount)
                {
                    var inGamePrivate = await _groupService.CreateInGamePrivateGroup(game);

                    connections.ForEach(async c =>
                    {
                        await ChangeGroup(inGamePrivate, c);
                        await _presenceHubContext.Clients.User(c.UserIdentifier).SendCoreAsync("ReceivePrivateGame", new object[] { game.Id.Value });
                    });
                }
            }
            else
            {
                await _playerService.ChangeInviteStatus(player, InviteStatus.REJECTED);
                await _gameService.Deactivate(game);

                var connections = await _connectionService.GetAll(inviteLobby.Id.Value);

                connections.ForEach(async c =>
                {
                    var mainLobby = await _groupService.GetOneUnique(GroupType.MAIN_LOBBY);
                    await ChangeGroup(mainLobby, c);

                    await _presenceHubContext.Clients.User(c.UserIdentifier).SendCoreAsync("ReceivePrivateGameInviteRejected", new object[] { game.Id.Value });
                });
            }
        }

        private async Task ChangeGroup(Group group, Connection connection)
        {
            await _connectionService.Save(connection, group, connection.UserIdentifier);
            await _presenceHubContext.Clients.User(connection.UserIdentifier).SendCoreAsync("ReceiveGroupType", new object[] { group.GroupType });
        }

        private async Task ChangeGroup(Group group, User user)
        {
            var connection = await _connectionService.GetOne(user.Id);
            await ChangeGroup(group, connection);
        }
    }
}
