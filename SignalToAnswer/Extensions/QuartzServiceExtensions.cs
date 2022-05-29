using Microsoft.Extensions.DependencyInjection;
using Quartz;
using SignalToAnswer.Jobs;

namespace SignalToAnswer.Extensions
{
    public static class QuartzServiceExtensions
    {
        public static void AddQuartz(this IServiceCollection services)
        {
            services.AddQuartz(q =>
            {
                q.UseMicrosoftDependencyInjectionJobFactory();

                AddPublicLobbyJob(q);
                AddDeactivateGuestJob(q);
                AddInviteLobbyJob(q);
        });

            services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
        }

        private static void AddPublicLobbyJob(IServiceCollectionQuartzConfigurator q)
        {
            var publicLobbyJobKey = new JobKey("PublicLobbyJob");

            q.AddJob<PublicLobbyJob>(opt => opt.WithIdentity(publicLobbyJobKey));

            q.AddTrigger(opts => opts.ForJob(publicLobbyJobKey)
                .WithIdentity("publicLobbyJob-trigger")
                .WithSimpleSchedule(x => x
                    .WithIntervalInSeconds(15)
                    .RepeatForever()));
        }

        private static void AddDeactivateGuestJob(IServiceCollectionQuartzConfigurator q)
        {
            var deactivateGuestJobKey = new JobKey("DeactivateGuestJob");

            q.AddJob<DeactivateGuestJob>(opt => opt.WithIdentity(deactivateGuestJobKey));

            q.AddTrigger(opts => opts.ForJob(deactivateGuestJobKey)
                .WithIdentity("deactivateGuestJob-trigger")
                .WithSimpleSchedule(x => x
                    .WithIntervalInSeconds(30)
                    .RepeatForever()));
        }

        private static void AddInviteLobbyJob(IServiceCollectionQuartzConfigurator q)
        {
            var inviteLobbyJobKey = new JobKey("InviteLobbyJob");

            q.AddJob<InviteLobbyJob>(opt => opt.WithIdentity(inviteLobbyJobKey));

            q.AddTrigger(opts => opts.ForJob(inviteLobbyJobKey)
                .WithIdentity("inviteLobbyJob-trigger")
                .WithSimpleSchedule(x => x
                    .WithIntervalInSeconds(10)
                    .RepeatForever()));
        }
    }
}
