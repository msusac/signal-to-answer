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
                AddDeactivateHostBotJob(q);
                AddLaunchGameJob(q);
                AddReplayGameJob(q);
                AddCancelGameJob(q);
                AddCancelGameReplayJob(q);
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

        private static void AddReplayGameJob(IServiceCollectionQuartzConfigurator q)
        {
            var replayGameJobKey = new JobKey("ReplayGameJob");

            q.AddJob<ReplayGameJob>(opt => opt.WithIdentity(replayGameJobKey));

            q.AddTrigger(opts => opts.ForJob(replayGameJobKey)
                .WithIdentity("replayGameJob-trigger")
                .WithSimpleSchedule(x => x
                    .WithIntervalInSeconds(10)
                    .RepeatForever()));
        }

        private static void AddCancelGameJob(IServiceCollectionQuartzConfigurator q)
        {
            var cancelGameJobKey = new JobKey("CancelGameJobKey");

            q.AddJob<CancelGameJob>(opt => opt.WithIdentity(cancelGameJobKey));

            q.AddTrigger(opts => opts.ForJob(cancelGameJobKey)
                .WithIdentity("cancelGameJob-trigger")
                .WithSimpleSchedule(x => x
                    .WithIntervalInSeconds(15)
                    .RepeatForever()));
        }

        private static void AddCancelGameReplayJob(IServiceCollectionQuartzConfigurator q)
        {
            var cancelGameReplayJobKey = new JobKey("CancelGameReplayJobKey");

            q.AddJob<CancelGameReplayJob>(opt => opt.WithIdentity(cancelGameReplayJobKey));

            q.AddTrigger(opts => opts.ForJob(cancelGameReplayJobKey)
                .WithIdentity("cancelGameReplayJob-trigger")
                .WithSimpleSchedule(x => x
                    .WithIntervalInSeconds(15)
                    .RepeatForever()));
        }
    }
}
