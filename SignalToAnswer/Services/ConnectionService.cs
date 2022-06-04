using Microsoft.Extensions.Logging;
using SignalToAnswer.Attributes;
using SignalToAnswer.Entities;
using SignalToAnswer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalToAnswer.Services
{
    public class ConnectionService
    {
        private readonly ILogger<ConnectionService> _logger;
        private readonly ConnectionRepository _connectionRepository;

        public ConnectionService(ILogger<ConnectionService> logger, ConnectionRepository connectionRepository)
        {
            _logger = logger;
            _connectionRepository = connectionRepository;
        }

        [Transactional]
        public async Task Save(Connection connection, Group group, string userIdentifier)
        {
            connection.Group = group;
            connection.UserIdentifier = userIdentifier;
            await _connectionRepository.Save(connection);

            _logger.LogInformation("User id {Id} switched to group: {GroupName}", connection.UserId, group.GroupName);
        }

        public async Task<List<Connection>> GetAll(int groupId)
        {
            return await _connectionRepository.FindAllByGroupId(groupId);
        } 

        public async Task<Connection> GetOne(Guid userId)
        {
            var connection = await _connectionRepository.FindOneByUser_Id(userId);

            if (connection == null)
            {
                throw new EntryPointNotFoundException("Selected connection does not exist!");
            }

            return connection;
        }

        public async Task<List<Connection>> GetTwoRandom(int groupId)
        {
            var connections = await _connectionRepository.FindAllByGroupId(groupId);

            return connections.OrderBy(a => Guid.NewGuid()).Take(2).ToList();
        }

        [Transactional]
        public async Task Deactivate(Connection connection)
        {
            connection.Active = false;
            await _connectionRepository.Save(connection);
        }
    }
}
