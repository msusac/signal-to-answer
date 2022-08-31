using Microsoft.Extensions.Logging;
using Quartz;
using SignalToAnswer.Attributes;
using SignalToAnswer.Constants;
using SignalToAnswer.Services;
using System;
using System.Threading.Tasks;

namespace SignalToAnswer.Jobs
{
    [DisallowConcurrentExecution]
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

            foreach (var u in users)
            {
                var connection = await _connectionService.GetOneQuietly(u.Id);

                if (connection == null || DateTime.Now.Subtract(connection.UpdatedAt).Minutes >= 90)
                {
                    if (connection != null)
                    {
                        await _connectionService.Deactivate(connection);
                    }
                    await _userService.Deactivate(u);
                    count++;
                }
            };

            if (count > 0)
            {
                _logger.LogInformation("{count} Guest account(s) deactivated.", count);
            }
        }
    }
}
