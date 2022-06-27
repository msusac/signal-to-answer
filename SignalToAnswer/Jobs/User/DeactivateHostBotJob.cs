using Microsoft.Extensions.Logging;
using Quartz;
using SignalToAnswer.Attributes;
using SignalToAnswer.Constants;
using SignalToAnswer.Services;
using System;
using System.Threading.Tasks;

namespace SignalToAnswer.Jobs.User
{
    [DisallowConcurrentExecution]
    public class DeactivateHostBotJob : IJob
    {
        private readonly ILogger<DeactivateGuestJob> _logger;
        private readonly UserService _userService;

        public DeactivateHostBotJob(ILogger<DeactivateGuestJob> logger, UserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        [Transactional]
        public async Task Execute(IJobExecutionContext context)
        {
            var users = await _userService.GetAll(RoleType.HOST_BOT);
            var count = 0;

            users.ForEach(async u =>
            {
                if (DateTime.Now.Subtract(u.CreatedAt).Minutes >= 35)
                {
                    await _userService.DeactivateAlt(u);
                    count++;
                }
            });

            if (count > 0)
            {
                _logger.LogInformation("{count} Host bot(s) deactivated.", count);
            }
        }
    }
}
