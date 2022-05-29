using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SignalToAnswer.Data;
using SignalToAnswer.Entities;
using System;
using System.Threading.Tasks;

namespace SignalToAnswer
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using var scope = host.Services.CreateScope();

            var services = scope.ServiceProvider;

            try
            {
                var dataContext = services.GetRequiredService<DataContext>();
                var roleManager = services.GetRequiredService<RoleManager<Role>>();
                var userManager = services.GetRequiredService<UserManager<User>>();

                await dataContext.Database.MigrateAsync();

                await Seed.SeedGroups(dataContext);
                await Seed.SeedRoles(roleManager);
                await Seed.SeedUsers(userManager);
                await Seed.SeedConnections(dataContext);
                await Seed.SeedTAQuestionCategories(dataContext);
                await Seed.SeedTAQuestionDifficulties(dataContext);
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error has occured during migration.");
            }

            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
