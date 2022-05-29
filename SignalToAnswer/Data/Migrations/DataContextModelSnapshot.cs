﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SignalToAnswer.Data;

#nullable disable

namespace SignalToAnswer.Data.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.2");

            modelBuilder.Entity("SignalToAnswer.Entities.Answer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("ANSWER_ID");

                    b.Property<bool?>("Active")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(true)
                        .HasColumnName("ACTIVE");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT")
                        .HasColumnName("CREATED_AT");

                    b.Property<int>("GameId")
                        .HasColumnType("INTEGER")
                        .HasColumnName("GAME_ID");

                    b.Property<bool>("IsCorrectAnswer")
                        .HasColumnType("INTEGER")
                        .HasColumnName("IS_CORRECT_ANSWER");

                    b.Property<int>("MatchId")
                        .HasColumnType("INTEGER")
                        .HasColumnName("MATCH_ID");

                    b.Property<int>("PlayerId")
                        .HasColumnType("INTEGER")
                        .HasColumnName("PLAYER_ID");

                    b.Property<int>("QuestionId")
                        .HasColumnType("INTEGER")
                        .HasColumnName("QUESTION_ID");

                    b.Property<int>("Score")
                        .HasColumnType("INTEGER")
                        .HasColumnName("SCORE");

                    b.Property<string>("SelectedAnswer")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("SELECTED_ANSWER");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("TEXT")
                        .HasColumnName("UPDATED_AT");

                    b.HasKey("Id");

                    b.HasIndex("GameId");

                    b.HasIndex("MatchId");

                    b.HasIndex("PlayerId");

                    b.HasIndex("QuestionId");

                    b.ToTable("ANSWERS", (string)null);
                });

            modelBuilder.Entity("SignalToAnswer.Entities.Connection", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("CONNECTION_ID");

                    b.Property<bool?>("Active")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(true)
                        .HasColumnName("ACTIVE");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT")
                        .HasColumnName("CREATED_AT");

                    b.Property<int>("GroupId")
                        .HasColumnType("INTEGER")
                        .HasColumnName("GROUP_ID");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("TEXT")
                        .HasColumnName("UPDATED_AT");

                    b.Property<Guid>("UserId")
                        .HasColumnType("TEXT")
                        .HasColumnName("USER_ID");

                    b.Property<string>("UserIdentifier")
                        .HasColumnType("TEXT")
                        .HasColumnName("USER_IDENTIFIER");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("CONNECTIONS", (string)null);
                });

            modelBuilder.Entity("SignalToAnswer.Entities.Game", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("GAME_ID");

                    b.Property<bool?>("Active")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(true)
                        .HasColumnName("ACTIVE");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT")
                        .HasColumnName("CREATED_AT");

                    b.Property<int>("GameStatus")
                        .HasColumnType("INTEGER")
                        .HasColumnName("GAME_STATUS");

                    b.Property<int>("GameType")
                        .HasColumnType("INTEGER")
                        .HasColumnName("GAME_TYPE");

                    b.Property<int>("MaxPlayerCount")
                        .HasColumnType("INTEGER")
                        .HasColumnName("MAX_PLAYER_COUNT");

                    b.Property<string>("QuestionCategories")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("QUESTION_CATEGORIES");

                    b.Property<int>("QuestionDifficulty")
                        .HasColumnType("INTEGER")
                        .HasColumnName("QUESTION_DIFFICULTY");

                    b.Property<int>("QuestionsCount")
                        .HasColumnType("INTEGER")
                        .HasColumnName("QUESTIONS_COUNT");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("TEXT")
                        .HasColumnName("UPDATED_AT");

                    b.HasKey("Id");

                    b.ToTable("GAMES", (string)null);
                });

            modelBuilder.Entity("SignalToAnswer.Entities.Group", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("GROUP_ID");

                    b.Property<bool?>("Active")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(true)
                        .HasColumnName("ACTIVE");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT")
                        .HasColumnName("CREATED_AT");

                    b.Property<string>("GroupName")
                        .IsRequired()
                        .HasColumnType("VARCHAR(100)")
                        .HasColumnName("GROUP_NAME");

                    b.Property<int>("GroupType")
                        .HasColumnType("INTEGER")
                        .HasColumnName("GROUP_TYPE");

                    b.Property<bool?>("IsUnique")
                        .IsRequired()
                        .HasColumnType("INTEGER")
                        .HasColumnName("IS_UNIQUE");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("TEXT")
                        .HasColumnName("UPDATED_AT");

                    b.HasKey("Id");

                    b.HasIndex("GroupName")
                        .IsUnique();

                    b.ToTable("GROUPS", (string)null);
                });

            modelBuilder.Entity("SignalToAnswer.Entities.Match", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("MATCH_ID");

                    b.Property<bool?>("Active")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(true)
                        .HasColumnName("ACTIVE");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT")
                        .HasColumnName("CREATED_AT");

                    b.Property<int>("GameId")
                        .HasColumnType("INTEGER")
                        .HasColumnName("GAME_ID");

                    b.Property<int>("MatchStatus")
                        .HasColumnType("INTEGER")
                        .HasColumnName("MATCH_STATUS");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("TEXT")
                        .HasColumnName("UPDATED_AT");

                    b.HasKey("Id");

                    b.HasIndex("GameId");

                    b.ToTable("MATCHES", (string)null);
                });

            modelBuilder.Entity("SignalToAnswer.Entities.Player", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("PLAYER_ID");

                    b.Property<bool?>("Active")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(true)
                        .HasColumnName("ACTIVE");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT")
                        .HasColumnName("CREATED_AT");

                    b.Property<int>("GameId")
                        .HasColumnType("INTEGER")
                        .HasColumnName("GAME_ID");

                    b.Property<int?>("InviteStatus")
                        .HasColumnType("INTEGER")
                        .HasColumnName("INVITE_STATUS");

                    b.Property<int>("PlayerStatus")
                        .HasColumnType("INTEGER")
                        .HasColumnName("PLAYER_STATUS");

                    b.Property<int?>("ReplayStatus")
                        .HasColumnType("INTEGER")
                        .HasColumnName("REPLAY_STATUS");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("TEXT")
                        .HasColumnName("UPDATED_AT");

                    b.Property<Guid>("UserId")
                        .HasColumnType("TEXT")
                        .HasColumnName("USER_ID");

                    b.HasKey("Id");

                    b.HasIndex("GameId");

                    b.HasIndex("UserId");

                    b.ToTable("PLAYERS", (string)null);
                });

            modelBuilder.Entity("SignalToAnswer.Entities.Question", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("QUESTION_ID");

                    b.Property<bool?>("Active")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(true)
                        .HasColumnName("ACTIVE");

                    b.Property<string>("AnswerChoices")
                        .IsRequired()
                        .HasColumnType("VARCHAR(500)")
                        .HasColumnName("ANSWER_CHOICES");

                    b.Property<int>("Attempts")
                        .HasColumnType("INTEGER")
                        .HasColumnName("ATTEMPTS");

                    b.Property<int>("Category")
                        .HasColumnType("VARCHAR(200)")
                        .HasColumnName("CATEGORY");

                    b.Property<string>("CorrectAnswer")
                        .IsRequired()
                        .HasColumnType("VARCHAR(200)")
                        .HasColumnName("CORRECT_ANSWER");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT")
                        .HasColumnName("CREATED_AT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("VARCHAR(500)")
                        .HasColumnName("DESCRIPTION");

                    b.Property<int>("Difficulty")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Finished")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(false)
                        .HasColumnName("IS_FINISHED");

                    b.Property<int>("GameId")
                        .HasColumnType("INTEGER")
                        .HasColumnName("GAME_ID");

                    b.Property<int>("MatchId")
                        .HasColumnType("INTEGER")
                        .HasColumnName("MATCH_ID");

                    b.Property<int>("ScoreMultiplier")
                        .HasColumnType("INTEGER")
                        .HasColumnName("SCORE_MULTIPLIER");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("TEXT")
                        .HasColumnName("UPDATED_AT");

                    b.HasKey("Id");

                    b.HasIndex("GameId");

                    b.HasIndex("MatchId");

                    b.ToTable("QUESTIONS", (string)null);
                });

            modelBuilder.Entity("SignalToAnswer.Entities.Result", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("RESULT_ID");

                    b.Property<bool?>("Active")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(true)
                        .HasColumnName("ACTIVE");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT")
                        .HasColumnName("CREATED_AT");

                    b.Property<int>("GameId")
                        .HasColumnType("INTEGER")
                        .HasColumnName("GAME_ID");

                    b.Property<bool>("IsWinner")
                        .HasColumnType("INTEGER")
                        .HasColumnName("IS_WINNER");

                    b.Property<int>("MatchId")
                        .HasColumnType("INTEGER")
                        .HasColumnName("MATCH_ID");

                    b.Property<string>("Note")
                        .HasColumnType("TEXT")
                        .HasColumnName("NOTE");

                    b.Property<int>("PlayerId")
                        .HasColumnType("INTEGER")
                        .HasColumnName("PLAYER_ID");

                    b.Property<int>("TotalScore")
                        .HasColumnType("INTEGER")
                        .HasColumnName("TOTAL_SCORE");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("TEXT")
                        .HasColumnName("UPDATED_AT");

                    b.HasKey("Id");

                    b.HasIndex("GameId");

                    b.HasIndex("MatchId");

                    b.HasIndex("PlayerId");

                    b.ToTable("RESULTS", (string)null);
                });

            modelBuilder.Entity("SignalToAnswer.Entities.Role", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT")
                        .HasColumnName("ROLE_ID");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("VARCHAR(100)")
                        .HasColumnName("ROLE_NAME");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("VARCHAR(100)")
                        .HasColumnName("ROLE_NAME_NORMALIZED");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("ROLES", (string)null);
                });

            modelBuilder.Entity("SignalToAnswer.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT")
                        .HasColumnName("USER_ID");

                    b.Property<bool?>("Active")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(true)
                        .HasColumnName("ACTIVE");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT")
                        .HasColumnName("CREATED_AT");

                    b.Property<string>("Description")
                        .HasColumnType("VARCHAR(500)")
                        .HasColumnName("DESCRIPTION");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("VARCHAT(256)")
                        .HasColumnName("EMAIL");

                    b.Property<string>("FullName")
                        .HasColumnType("VARCHAR(100)")
                        .HasColumnName("FULLNAME");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("VARCHAR(256)")
                        .HasColumnName("EMAIL_NORMALIZED");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("VARCHAR(100)")
                        .HasColumnName("USERNAME_NORMALIZED");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("TEXT")
                        .HasColumnName("PASSWORD_HASH");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("TEXT")
                        .HasColumnName("UPDATED_AT");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("VARCHAR(100)")
                        .HasColumnName("USERNAME");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("USERS", (string)null);
                });

            modelBuilder.Entity("SignalToAnswer.Entities.UserRole", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("TEXT")
                        .HasColumnName("USER_ID");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("TEXT")
                        .HasColumnName("ROLE_ID");

                    b.Property<bool?>("Active")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(true)
                        .HasColumnName("ACTIVE");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT")
                        .HasColumnName("CREATED_AT");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("TEXT")
                        .HasColumnName("UPDATED_AT");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("O2O_USER_ROLE", (string)null);
                });

            modelBuilder.Entity("SignalToAnswer.Integrations.TriviaApi.Entities.TAQuestionCategory", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("TA_QUESTION_CATEGORY_ID");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("VARCHAR(200)")
                        .HasColumnName("NAME");

                    b.Property<string>("Param")
                        .IsRequired()
                        .HasColumnType("VARCHAR(200)")
                        .HasColumnName("PARAM");

                    b.HasKey("Id");

                    b.ToTable("TA_QUESTION_CATEGORIES", (string)null);
                });

            modelBuilder.Entity("SignalToAnswer.Integrations.TriviaApi.Entities.TAQuestionDifficulty", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("TA_QUESTION_DIFFICULTY_ID");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("VARCHAR(200)")
                        .HasColumnName("NAME");

                    b.Property<string>("Param")
                        .IsRequired()
                        .HasColumnType("VARCHAR(200)")
                        .HasColumnName("PARAM");

                    b.HasKey("Id");

                    b.ToTable("TA_QUESTION_DIFFICULTY", (string)null);
                });

            modelBuilder.Entity("SignalToAnswer.Entities.Answer", b =>
                {
                    b.HasOne("SignalToAnswer.Entities.Game", "Game")
                        .WithMany("Answers")
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SignalToAnswer.Entities.Match", "Match")
                        .WithMany("Answers")
                        .HasForeignKey("MatchId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SignalToAnswer.Entities.Player", "Player")
                        .WithMany("Answers")
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SignalToAnswer.Entities.Question", "Question")
                        .WithMany("Answers")
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Game");

                    b.Navigation("Match");

                    b.Navigation("Player");

                    b.Navigation("Question");
                });

            modelBuilder.Entity("SignalToAnswer.Entities.Connection", b =>
                {
                    b.HasOne("SignalToAnswer.Entities.Group", "Group")
                        .WithMany("Connections")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SignalToAnswer.Entities.User", "User")
                        .WithOne("Connection")
                        .HasForeignKey("SignalToAnswer.Entities.Connection", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Group");

                    b.Navigation("User");
                });

            modelBuilder.Entity("SignalToAnswer.Entities.Match", b =>
                {
                    b.HasOne("SignalToAnswer.Entities.Game", "Game")
                        .WithMany("Matches")
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Game");
                });

            modelBuilder.Entity("SignalToAnswer.Entities.Player", b =>
                {
                    b.HasOne("SignalToAnswer.Entities.Game", "Game")
                        .WithMany("Players")
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SignalToAnswer.Entities.User", "User")
                        .WithMany("Players")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Game");

                    b.Navigation("User");
                });

            modelBuilder.Entity("SignalToAnswer.Entities.Question", b =>
                {
                    b.HasOne("SignalToAnswer.Entities.Game", "Game")
                        .WithMany("Questions")
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SignalToAnswer.Entities.Match", "Match")
                        .WithMany("Questions")
                        .HasForeignKey("MatchId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Game");

                    b.Navigation("Match");
                });

            modelBuilder.Entity("SignalToAnswer.Entities.Result", b =>
                {
                    b.HasOne("SignalToAnswer.Entities.Game", "Game")
                        .WithMany("Result")
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SignalToAnswer.Entities.Match", "Match")
                        .WithMany("Results")
                        .HasForeignKey("MatchId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SignalToAnswer.Entities.Player", "Player")
                        .WithMany("Results")
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Game");

                    b.Navigation("Match");

                    b.Navigation("Player");
                });

            modelBuilder.Entity("SignalToAnswer.Entities.UserRole", b =>
                {
                    b.HasOne("SignalToAnswer.Entities.Role", "Role")
                        .WithMany("UserRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SignalToAnswer.Entities.User", "User")
                        .WithOne("UserRole")
                        .HasForeignKey("SignalToAnswer.Entities.UserRole", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");

                    b.Navigation("User");
                });

            modelBuilder.Entity("SignalToAnswer.Entities.Game", b =>
                {
                    b.Navigation("Answers");

                    b.Navigation("Matches");

                    b.Navigation("Players");

                    b.Navigation("Questions");

                    b.Navigation("Result");
                });

            modelBuilder.Entity("SignalToAnswer.Entities.Group", b =>
                {
                    b.Navigation("Connections");
                });

            modelBuilder.Entity("SignalToAnswer.Entities.Match", b =>
                {
                    b.Navigation("Answers");

                    b.Navigation("Questions");

                    b.Navigation("Results");
                });

            modelBuilder.Entity("SignalToAnswer.Entities.Player", b =>
                {
                    b.Navigation("Answers");

                    b.Navigation("Results");
                });

            modelBuilder.Entity("SignalToAnswer.Entities.Question", b =>
                {
                    b.Navigation("Answers");
                });

            modelBuilder.Entity("SignalToAnswer.Entities.Role", b =>
                {
                    b.Navigation("UserRoles");
                });

            modelBuilder.Entity("SignalToAnswer.Entities.User", b =>
                {
                    b.Navigation("Connection");

                    b.Navigation("Players");

                    b.Navigation("UserRole");
                });
#pragma warning restore 612, 618
        }
    }
}
