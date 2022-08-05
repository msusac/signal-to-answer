using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SignalToAnswer.Entities;
using SignalToAnswer.Integrations.TriviaApi.Entities;
using System;
using System.Collections.Generic;

namespace SignalToAnswer.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static void BuildAnswerEntity(this ModelBuilder builder)
        {
            builder.Entity<Answer>(b =>
            {
                b.ToTable("ANSWERS");

                b.Property(a => a.Id)
                .HasColumnName("ANSWER_ID")
                .IsRequired();

                b.Property(a => a.GameId)
                .HasColumnName("GAME_ID")
                .IsRequired();

                b.Property(a => a.MatchId)
                .HasColumnName("MATCH_ID")
                .IsRequired();

                b.Property(a => a.PlayerId)
                .HasColumnName("PLAYER_ID")
                .IsRequired();

                b.Property(a => a.QuestionId)
                .HasColumnName("QUESTION_ID")
                .IsRequired();

                b.Property(a => a.SelectedAnswer)
                .HasColumnName("SELECTED_ANSWER");

                b.Property(a => a.SelectedAnswerIndex)
                .HasColumnName("SELECTED_ANSWER_INDEX");

                b.Property(a => a.IsCorrectAnswer)
                .HasColumnName("IS_CORRECT_ANSWER")
                .IsRequired();

                b.Property(a => a.Score)
                .HasColumnName("SCORE")
                .IsRequired();

                b.Property(a => a.CreatedAt)
                .HasColumnName("CREATED_AT")
                .IsRequired();

                b.Property(a => a.UpdatedAt)
                .HasColumnName("UPDATED_AT")
                .IsRequired();

                b.Property(a => a.Active)
                .HasColumnName("ACTIVE")
                .HasDefaultValue(true)
                .IsRequired();

                b.HasKey(a => a.Id);
            });
        }

        public static void BuildConnectionEntity(this ModelBuilder builder)
        {
            builder.Entity<Connection>(b =>
            {
                b.ToTable("CONNECTIONS");

                b.Property(c => c.Id)
                .HasColumnName("CONNECTION_ID")
                .IsRequired();

                b.Property(c => c.GroupId)
                .HasColumnName("GROUP_ID")
                .IsRequired();

                b.Property(c => c.UserId)
               .HasColumnName("USER_ID")
               .IsRequired();

                b.Property(c => c.UserIdentifier)
                .HasColumnName("USER_IDENTIFIER");

                b.Property(c => c.CreatedAt)
                .HasColumnName("CREATED_AT")
                .IsRequired();

                b.Property(c => c.UpdatedAt)
                .HasColumnName("UPDATED_AT")
                .IsRequired();

                b.Property(c => c.Active)
                .HasColumnName("ACTIVE")
                .HasDefaultValue(true)
                .IsRequired();

                b.HasKey(c => c.Id);
            });
        }

        public static void BuildGameEntity(this ModelBuilder builder)
        {
            builder.Entity<Game>(b =>
            {
                b.ToTable("GAMES");

                b.Property(g => g.Id)
                .HasColumnName("GAME_ID")
                .IsRequired();

                b.Property(g => g.GameType)
                .HasColumnName("GAME_TYPE")
                .IsRequired();

                b.Property(g => g.GameStatus)
                .HasColumnName("GAME_STATUS")
                .IsRequired();

                b.Property(g => g.MaxPlayerCount)
                .HasColumnName("MAX_PLAYER_COUNT")
                .IsRequired();

                b.Property(g => g.QuestionsCount)
                .HasColumnName("QUESTIONS_COUNT")
                .IsRequired();

                b.Property(g => g.QuestionCategories)
                .HasColumnName("QUESTION_CATEGORIES")
                .IsRequired()
                .HasConversion(
                    q => JsonConvert.SerializeObject(q),
                    q => JsonConvert.DeserializeObject<List<int>>(q)
                );

                b.Property(g => g.QuestionDifficulty)
                .HasColumnName("QUESTION_DIFFICULTY")
                .IsRequired();

                b.Property(g => g.CreatedAt)
                .HasColumnName("CREATED_AT")
                .IsRequired();

                b.Property(g => g.UpdatedAt)
                .HasColumnName("UPDATED_AT")
                .IsRequired();

                b.Property(g => g.Active)
                .HasColumnName("ACTIVE")
                .HasDefaultValue(true)
                .IsRequired();

                b.HasMany(g => g.Answers)
                .WithOne(a => a.Game)
                .HasForeignKey(a => a.GameId)
                .IsRequired();

                b.HasMany(g => g.Matches)
                .WithOne(m => m.Game)
                .HasForeignKey(m => m.GameId)
                .IsRequired();

                b.HasMany(g => g.Players)
                .WithOne(p => p.Game)
                .HasForeignKey(p => p.GameId)
                .IsRequired();

                b.HasMany(g => g.Questions)
                .WithOne(q => q.Game)
                .HasForeignKey(q => q.GameId)
                .IsRequired();

                b.HasMany(g => g.Result)
                .WithOne(r => r.Game)
                .HasForeignKey(r => r.GameId)
                .IsRequired();

                b.HasKey(g => g.Id);
            });
        }

        public static void BuildGroupEntity(this ModelBuilder builder)
        {
            builder.Entity<Group>(b =>
            {
                b.ToTable("GROUPS");

                b.Property(g => g.Id)
                .HasColumnName("GROUP_ID")
                .IsRequired();

                b.Property(g => g.GroupName)
                .HasColumnName("GROUP_NAME")
                .HasColumnType("VARCHAR(100)")
                .IsRequired();

                b.Property(g => g.GroupType)
                .HasColumnName("GROUP_TYPE")
                .IsRequired();

                b.Property(g => g.IsUnique)
                .HasColumnName("IS_UNIQUE")
                .IsRequired();

                b.Property(g => g.CreatedAt)
                .HasColumnName("CREATED_AT")
                .IsRequired();

                b.Property(g => g.UpdatedAt)
                .HasColumnName("UPDATED_AT")
                .IsRequired();

                b.Property(g => g.Active)
                .HasColumnName("ACTIVE")
                .HasDefaultValue(true)
                .IsRequired();

                b.HasMany(g => g.Connections)
                .WithOne(ug => ug.Group)
                .HasForeignKey(ug => ug.GroupId)
                .IsRequired();

                b.HasIndex(g => g.GroupName)
                .IsUnique();

                b.HasKey(g => g.Id);
            });
        }

        public static void BuildMatchEntity(this ModelBuilder builder)
        {
            builder.Entity<Match>(b =>
            {
                b.ToTable("MATCHES");

                b.Property(m => m.Id)
                .HasColumnName("MATCH_ID")
                .IsRequired();

                b.Property(m => m.GameId)
                .HasColumnName("GAME_ID")
                .IsRequired();

                b.Property(m => m.IsOngoing)
                .HasColumnName("IS_ONGOING")
                .HasDefaultValue(true)
                .IsRequired();

                b.Property(m => m.CreatedAt)
                .HasColumnName("CREATED_AT")
                .IsRequired();

                b.Property(m => m.UpdatedAt)
                .HasColumnName("UPDATED_AT")
                .IsRequired();

                b.Property(m => m.Active)
                .HasColumnName("ACTIVE")
                .HasDefaultValue(true)
                .IsRequired();

                b.HasMany(m => m.Answers)
                .WithOne(a => a.Match)
                .HasForeignKey(a => a.MatchId)
                .IsRequired();

                b.HasMany(m => m.Questions)
                .WithOne(q => q.Match)
                .HasForeignKey(q => q.MatchId)
                .IsRequired();

                b.HasMany(m => m.Results)
                .WithOne(r => r.Match)
                .HasForeignKey(r => r.MatchId)
                .IsRequired();

                b.HasKey(m => m.Id);
            });
        }

        public static void BuildPlayerEntity(this ModelBuilder builder)
        {
            builder.Entity<Player>(b =>
            {
                b.ToTable("PLAYERS");

                b.Property(p => p.Id)
               .HasColumnName("PLAYER_ID")
               .IsRequired();

                b.Property(p => p.GameId)
               .HasColumnName("GAME_ID")
               .IsRequired();

                b.Property(p => p.UserId)
                .HasColumnName("USER_ID")
                .IsRequired();

                b.Property(p => p.PlayerStatus)
                .HasColumnName("PLAYER_STATUS")
                .IsRequired();

                b.Property(p => p.ReplayStatus)
                .HasColumnName("REPLAY_STATUS");

                b.Property(p => p.InviteStatus)
                .HasColumnName("INVITE_STATUS");

                b.Property(p => p.CreatedAt)
                .HasColumnName("CREATED_AT")
                .IsRequired();

                b.Property(p => p.UpdatedAt)
                .HasColumnName("UPDATED_AT")
                .IsRequired();

                b.Property(p => p.Active)
                .HasColumnName("ACTIVE")
                .HasDefaultValue(true)
                .IsRequired();

                b.HasMany(p => p.Answers)
                .WithOne(a => a.Player)
                .HasForeignKey(a => a.PlayerId)
                .IsRequired();

                b.HasMany(p => p.Results)
                .WithOne(r => r.Player)
                .HasForeignKey(r => r.PlayerId)
                .IsRequired();

                b.HasKey(p => p.Id);
            });
        }

        public static void BuildQuestionEntity(this ModelBuilder builder)
        {
            builder.Entity<Question>(b =>
            {
                b.ToTable("QUESTIONS");

                b.Property(q => q.Id)
                .HasColumnName("QUESTION_ID")
                .IsRequired();

                b.Property(q => q.Row)
                .HasColumnName("ROW")
                .IsRequired();

                b.Property(q => q.GameId)
                .HasColumnName("GAME_ID")
                .IsRequired();

                b.Property(q => q.MatchId)
                .HasColumnName("MATCH_ID")
                .IsRequired();

                b.Property(q => q.Category)
                .HasColumnName("CATEGORY")
                .HasColumnType("VARCHAR(200)")
                .IsRequired();

                b.Property(q => q.ScoreMultiplier)
                .HasColumnName("SCORE_MULTIPLIER")
                .IsRequired();

                b.Property(q => q.Description)
                .HasColumnName("DESCRIPTION")
                .HasColumnType("VARCHAR(500)")
                .IsRequired();

                b.Property(q => q.CorrectAnswer)
                .HasColumnName("CORRECT_ANSWER")
                .HasColumnType("VARCHAR(200)")
                .IsRequired();

                b.Property(q => q.CorrectAnswerIndex)
                .HasColumnName("CORRECT_ANSWER_INDEX")
                .IsRequired();

                b.Property(q => q.RemainingTime)
                .HasColumnName("REMAINING_TIME")
                .IsRequired();

                b.Property(q => q.AnswerChoices)
                .HasColumnName("ANSWER_CHOICES")
                .HasColumnType("VARCHAR(500)")
                .IsRequired()
                .HasConversion(
                    q => JsonConvert.SerializeObject(q),
                    q => JsonConvert.DeserializeObject<List<string>>(q)
                );

                b.Property(g => g.IsOngoing)
                .HasColumnName("IS_ONGOING")
                .HasDefaultValue(true)
                .IsRequired();

                b.Property(q => q.CreatedAt)
                .HasColumnName("CREATED_AT")
                .IsRequired();

                b.Property(q => q.UpdatedAt)
                .HasColumnName("UPDATED_AT")
                .IsRequired();

                b.Property(q => q.Active)
                .HasColumnName("ACTIVE")
                .HasDefaultValue(true)
                .IsRequired();

                b.HasMany(q => q.Answers)
                .WithOne(a => a.Question)
                .HasForeignKey(a => a.QuestionId)
                .IsRequired();

                b.HasKey(q => q.Id);
            });
        }

        public static void BuildRoleEntity(this ModelBuilder builder)
        {
            builder.Entity<Role>(b =>
            {
                b.ToTable("ROLES");

                b.Property(r => r.Id)
                .HasColumnName("ROLE_ID")
                .IsRequired();

                b.Property(r => r.Name)
                .HasColumnName("ROLE_NAME")
                .HasColumnType("VARCHAR(100)")
                .IsRequired();

                b.Property(r => r.NormalizedName)
                .HasColumnName("ROLE_NAME_NORMALIZED")
                .HasColumnType("VARCHAR(100)");

                b.HasMany(r => r.UserRoles)
                .WithOne(ur => ur.Role)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();

                b.Ignore(r => r.ConcurrencyStamp);
            });
        }

        public static void BuildResultEntity(this ModelBuilder builder)
        {
            builder.Entity<Result>(b =>
            {
                b.ToTable("RESULTS");

                b.Property(r => r.Id)
                .HasColumnName("RESULT_ID")
                .IsRequired();

                b.Property(r => r.GameId)
                .HasColumnName("GAME_ID")
                .IsRequired();

                b.Property(r => r.MatchId)
                .HasColumnName("MATCH_ID")
                .IsRequired();

                b.Property(r => r.PlayerId)
                .HasColumnName("PLAYER_ID")
                .IsRequired();

                b.Property(r => r.TotalScore)
                .HasColumnName("TOTAL_SCORE")
                .IsRequired();

                b.Property(r => r.WinnerStatus)
                .HasColumnName("WINNER_STATUS");

                b.Property(r => r.Note)
                .HasColumnName("NOTE");

                b.Property(r => r.CreatedAt)
                .HasColumnName("CREATED_AT")
                .IsRequired();

                b.Property(r => r.UpdatedAt)
                .HasColumnName("UPDATED_AT")
                .IsRequired();

                b.Property(r => r.Active)
                .HasColumnName("ACTIVE")
                .HasDefaultValue(true)
                .IsRequired();

                b.HasKey(r => r.Id);
            });
        }

        public static void BuildUserEntity(this ModelBuilder builder)
        {
            builder.Entity<User>(b =>
            {
                b.ToTable("USERS");

                b.Property(u => u.Id)
                .HasColumnName("USER_ID")
                .IsRequired();

                b.Property(u => u.UserName)
                .HasColumnName("USERNAME")
                .HasColumnType("VARCHAR(100)")
                .IsRequired();

                b.Property(u => u.NormalizedUserName)
                .HasColumnName("USERNAME_NORMALIZED")
                .HasColumnType("VARCHAR(100)");

                b.Property(u => u.Email)
                .HasColumnName("EMAIL")
                .HasColumnType("VARCHAR(256)");

                b.Property(u => u.NormalizedEmail)
                .HasColumnName("EMAIL_NORMALIZED")
                .HasColumnType("VARCHAR(256)");

                b.Property(u => u.PasswordHash)
               .HasColumnName("PASSWORD_HASH");

                b.Property(u => u.CreatedAt)
                .HasColumnName("CREATED_AT")
                .IsRequired();

                b.Property(u => u.UpdatedAt)
                .HasColumnName("UPDATED_AT")
                .IsRequired();

                b.Property(u => u.Active)
                .HasColumnName("ACTIVE")
                .HasDefaultValue(true)
                .IsRequired();

                b.HasMany(u => u.Players)
                .WithOne(p => p.User)
                .HasForeignKey(p => p.UserId);

                b.HasOne(u => u.Connection)
                .WithOne(gu => gu.User)
                .HasForeignKey<Connection>(ug => ug.UserId)
                .IsRequired();

                b.HasOne(u => u.UserRole)
                .WithOne(ur => ur.User)
                .HasForeignKey<UserRole>(ur => ur.UserId)
                .IsRequired();

                b.Ignore(u => u.AccessFailedCount)
                .Ignore(u => u.ConcurrencyStamp)
                .Ignore(u => u.EmailConfirmed)
                .Ignore(u => u.LockoutEnabled)
                .Ignore(u => u.LockoutEnd)
                .Ignore(u => u.Password)
                .Ignore(u => u.PhoneNumber)
                .Ignore(u => u.PhoneNumberConfirmed)
                .Ignore(u => u.Role)
                .Ignore(u => u.SecurityStamp)
                .Ignore(u => u.TwoFactorEnabled);
            });
        }

        public static void BuildUserRoleEntity(this ModelBuilder builder)
        {
            builder.Entity<UserRole>(b =>
            {
                b.ToTable("O2O_USER_ROLE");

                b.Property(ur => ur.UserId)
                .HasColumnName("USER_ID")
                .IsRequired();

                b.Property(ur => ur.RoleId)
                .HasColumnName("ROLE_ID")
                .IsRequired();

                b.Property(ur => ur.CreatedAt)
                .HasColumnName("CREATED_AT")
                .IsRequired();

                b.Property(ur => ur.UpdatedAt)
                .HasColumnName("UPDATED_AT")
                .IsRequired();

                b.Property(ur => ur.Active)
                .HasColumnName("ACTIVE")
                .HasDefaultValue(true)
                .IsRequired();
            });
        }

        public static void BuildTAQuestionCategoryEntity(this ModelBuilder builder)
        {
            builder.Entity<TAQuestionCategory>(b =>
            {
                b.ToTable("TA_QUESTION_CATEGORIES");

                b.Property(g => g.Id)
               .HasColumnName("TA_QUESTION_CATEGORY_ID")
               .IsRequired();

                b.Property(g => g.Name)
                .HasColumnName("NAME")
                .HasColumnType("VARCHAR(200)")
                .IsRequired();

                b.Property(g => g.Param)
                .HasColumnName("PARAM")
                .HasColumnType("VARCHAR(200)")
                .IsRequired();

                b.HasKey(b => b.Id);
            });
        }

        public static void BuildTAQuestionDifficultyEntity(this ModelBuilder builder)
        {
            builder.Entity<TAQuestionDifficulty>(b =>
            {
                b.ToTable("TA_QUESTION_DIFFICULTY");

                b.Property(g => g.Id)
               .HasColumnName("TA_QUESTION_DIFFICULTY_ID")
               .IsRequired();

                b.Property(g => g.Name)
                .HasColumnName("NAME")
                .HasColumnType("VARCHAR(200)")
                .IsRequired();

                b.Property(g => g.Param)
                .HasColumnName("PARAM")
                .HasColumnType("VARCHAR(200)")
                .IsRequired();

                b.HasKey(b => b.Id);
            });
        }

        public static void BuildOtherEntities(this ModelBuilder builder)
        {
            builder.Ignore<IdentityRoleClaim<Guid>>()
            .Ignore<IdentityUserClaim<Guid>>()
            .Ignore<IdentityUserLogin<Guid>>()
            .Ignore<IdentityUserToken<Guid>>();
        }
    }
}