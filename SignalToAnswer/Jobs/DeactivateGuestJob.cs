using Microsoft.Extensions.Logging;
using Quartz;
using SignalToAnswer.Attributes;
using SignalToAnswer.Constants;
using SignalToAnswer.Services;
using System;
using System.Threading.Tasks;

namespace SignalToAnswer.Jobs
{
    public class DeactivateGuestJob : IJob
    {
        private readonly ILogger<DeactivateGuestJob> _logger;
        private readonly ConnectionService _connectionService;
        private readonly GroupService _groupService;
        private readonly UserService _userService;

        public DeactivateGuestJob(ILogger<DeactivateGuestJob> logger, ConnectionService connectionService, GroupService groupService, UserService userService)
        {
            _logger = logger;
            _connectionService = connectionService;
            _groupService = groupService;
            _userService = userService;
        }

        [Transactional]
        public async Task Execute(IJobExecutionContext context)
        {
            var group = await _groupService.GetOneUnique(GroupType.OFFLINE);
            var users = await _userService.GetAll(RoleType.GUEST, group.Id.Value);
            var count = 0;

            foreach (var user in users)
            {
                var connection = await _connectionService.GetOne(user.Id);

                // Deactivate guest account if it is offline more than 24 hours
                if (DateTime.Now.Subtract(connection.UpdatedAt).Hours > 1)
                {
                    await _connectionService.Deactivate(connection);
                    await _userService.Deactivate(user);
                    count++;
                }
            }

            if (count > 0)
            {
                _logger.LogInformation("{count} Guest account deactivated.", count);
            }
        }
    }
}
