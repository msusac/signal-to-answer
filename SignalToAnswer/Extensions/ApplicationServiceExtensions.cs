﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SignalToAnswer.Data;
using SignalToAnswer.Facades;
using SignalToAnswer.Hubs;
using SignalToAnswer.Integrations.TriviaApi.Mappers;
using SignalToAnswer.Integrations.TriviaApi.Repositories;
using SignalToAnswer.Integrations.TriviaApi.Services;
using SignalToAnswer.Mappers.Dtos;
using SignalToAnswer.Mappers.Option;
using SignalToAnswer.Repositories;
using SignalToAnswer.Services;
using SignalToAnswer.Validation;
using SignalToAnswer.Validators.Form;

namespace SignalToAnswer.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            AddCors(services);
            AddDbContext(services, config);
            AddScoped(services);
            AddTransient(services);

            return services;
        }

        private static void AddCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.WithOrigins("http://localhost:3200")
                        .AllowAnyHeader()
                        .AllowCredentials()
                        .AllowAnyMethod();
                });
            });
        }

        private static void AddDbContext(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlite(config.GetConnectionString("DefaultConnection"));
            });
        }

        private static void AddScoped(this IServiceCollection services)
        {
            AddScopedFacade(services);
            AddScopedMapper(services);
            AddScopedRepository(services);
            AddScopedService(services);
            AddScopedValidator(services);
        }

        private static void AddScopedFacade(this IServiceCollection services)
        {
            services.AddScoped<AccountFacade>();
            services.AddScoped<GameFacade>();
            services.AddScoped<ListFacade>();
        }

        private static void AddScopedMapper(this IServiceCollection services)
        {
            services.AddScoped<GameDtoMapper>();
            services.AddScoped<UserDtoMapper>();
            services.AddScoped<UserOptionMapper>();
            services.AddScoped<TARequestMapper>();
            services.AddScoped<QuestionMapper>();
        }

        private static void AddScopedRepository(this IServiceCollection services)
        {
            services.AddScoped<ConnectionRepository>();
            services.AddScoped<GameRepository>();
            services.AddScoped<GroupRepository>();
            services.AddScoped<PlayerRepository>();
            services.AddScoped<TAQuestionCategoryRepository>();
            services.AddScoped<TAQuestionDifficultyRepository>();
            services.AddScoped<QuestionRepository>();
            services.AddScoped<RoleRepository>();
            services.AddScoped<UserRepository>();
        }

        private static void AddScopedService(this IServiceCollection services)
        {
            services.AddScoped<ConnectionService>();
            services.AddScoped<GameService>();
            services.AddScoped<GroupService>();
            services.AddScoped<PlayerService>();
            services.AddScoped<QuestionService>();
            services.AddHttpClient<TAService>();
            services.AddScoped<TokenService>();
            services.AddScoped<UserService>();
        }

        private static void AddScopedValidator(this IServiceCollection services)
        {
            services.AddScoped<CreateGameFormValidator>();
            services.AddScoped<InviteResponseFormValidator>();
            services.AddScoped<LoginFormValidator>();
            services.AddScoped<RegisterFormValidator>();
        }

        private static void AddTransient(this IServiceCollection services)
        {
            services.AddTransient<GameHub>();
            services.AddTransient<PresenceHub>();
            services.AddTransient<ValidationManager>();
        }
    }
}