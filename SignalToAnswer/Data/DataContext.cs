using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SignalToAnswer.Entities;
using SignalToAnswer.Extensions;
using SignalToAnswer.Integrations.TriviaApi.Entities;
using System;

namespace SignalToAnswer.Data
{
    public class DataContext : IdentityDbContext<User, Role, Guid, IdentityUserClaim<Guid>, UserRole,
        IdentityUserLogin<Guid>, IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Answer> Answers { get; set; }

        public DbSet<Connection> Connections { get; set; }

        public DbSet<Game> Games { get; set; }

        public DbSet<Group> Groups { get; set; }

        public DbSet<Match> Matches { get; set; }

        public DbSet<Player> Players { get; set; }

        public DbSet<Question> Questions { get; set; }

        public DbSet<Result> Results { get; set; }

        public DbSet<TAQuestionCategory> TAQuestionCategories { get; set; }

        public DbSet<TAQuestionDifficulty> TAQuestionDifficulty { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.BuildAnswerEntity();
            builder.BuildConnectionEntity();
            builder.BuildGameEntity();
            builder.BuildGroupEntity();
            builder.BuildMatchEntity();
            builder.BuildPlayerEntity();
            builder.BuildQuestionEntity();
            builder.BuildResultEntity();
            builder.BuildRoleEntity();
            builder.BuildUserEntity();
            builder.BuildUserRoleEntity();
            builder.BuildTAQuestionCategoryEntity();
            builder.BuildTAQuestionDifficultyEntity();
            builder.BuildOtherEntities();
        }
    }
}
