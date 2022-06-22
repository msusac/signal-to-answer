using Microsoft.Extensions.DependencyInjection;
using Quartz;
using SignalToAnswer.Jobs;
using SignalToAnswer.Jobs.User;

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
                AddInviteLobbyJob(q);
                AddDeactivateGuestJob(q);
                AddLaunchGameJob(q);
                AddDeactivateHostBotJob(q);
                AddDeactivateGameJob(q);
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

        private static void AddLaunchGameJob(IServiceCollectionQuartzConfigurator q)
        {
            var launchGameJobKey = new JobKey("LaunchGameJob");

            q.AddJob<LaunchGameJob>(opt => opt.WithIdentity(launchGameJobKey));

            q.AddTrigger(opts => opts.ForJob(launchGameJobKey)
                .WithIdentity("launchGameJob-trigger")
                .WithSimpleSchedule(x => x
                    .WithIntervalInSeconds(10)
                    .RepeatForever()));
        }

        private static void AddDeactivateHostBotJob(IServiceCollectionQuartzConfigurator q)
        {
            var deactivateHostBotJobKey = new JobKey("DeactivateHostBotJob");

            q.AddJob<DeactivateHostBotJob>(opt => opt.WithIdentity(deactivateHostBotJobKey));

            q.AddTrigger(opts => opts.ForJob(deactivateHostBotJobKey)
                .WithIdentity("deactivateHostBotJob-trigger")
                .WithSimpleSchedule(x => x
                    .WithIntervalInSeconds(30)
                    .RepeatForever()));
        }

        private static void AddDeactivateGameJob(IServiceCollectionQuartzConfigurator q)
        {
            var deactivateGameJobKey = new JobKey("DeactivateGameJobKey");

            q.AddJob<DeactivateGameJob>(opt => opt.WithIdentity(deactivateGameJobKey));

            q.AddTrigger(opts => opts.ForJob(deactivateGameJobKey)
                .WithIdentity("deactivateGameJobJob-trigger")
                .WithSimpleSchedule(x => x
                    .WithIntervalInSeconds(30)
                    .RepeatForever()));
        }
    }
}
