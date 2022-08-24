using Microsoft.AspNetCore.SignalR;
using SignalToAnswer.Entities;
using System.Threading.Tasks;

namespace SignalToAnswer.Hubs.Contexts
{
    public class GameHubContext
    {
        private readonly IHubContext<GameHub, IGameHub> _gameHubContext;

        public GameHubContext(IHubContext<GameHub, IGameHub> gameHubContext)
        {
            _gameHubContext = gameHubContext;
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
    }
}
