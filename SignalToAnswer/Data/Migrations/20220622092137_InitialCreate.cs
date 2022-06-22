using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SignalToAnswer.Data.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GAMES",
                columns: table => new
                {
                    GAME_ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    GAME_TYPE = table.Column<int>(type: "INTEGER", nullable: false),
                    GAME_STATUS = table.Column<int>(type: "INTEGER", nullable: false),
                    MAX_PLAYER_COUNT = table.Column<int>(type: "INTEGER", nullable: false),
                    QUESTIONS_COUNT = table.Column<int>(type: "INTEGER", nullable: false),
                    QUESTION_CATEGORIES = table.Column<string>(type: "TEXT", nullable: false),
                    QUESTION_DIFFICULTY = table.Column<int>(type: "INTEGER", nullable: false),
                    CREATED_AT = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UPDATED_AT = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ACTIVE = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GAMES", x => x.GAME_ID);
                });

            migrationBuilder.CreateTable(
                name: "GROUPS",
                columns: table => new
                {
                    GROUP_ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    GROUP_NAME = table.Column<string>(type: "VARCHAR(100)", nullable: false),
                    GROUP_TYPE = table.Column<int>(type: "INTEGER", nullable: false),
                    IS_UNIQUE = table.Column<bool>(type: "INTEGER", nullable: false),
                    CREATED_AT = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UPDATED_AT = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ACTIVE = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GROUPS", x => x.GROUP_ID);
                });

            migrationBuilder.CreateTable(
                name: "ROLES",
                columns: table => new
                {
                    ROLE_ID = table.Column<Guid>(type: "TEXT", nullable: false),
                    ROLE_NAME = table.Column<string>(type: "VARCHAR(100)", maxLength: 256, nullable: false),
                    ROLE_NAME_NORMALIZED = table.Column<string>(type: "VARCHAR(100)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ROLES", x => x.ROLE_ID);
                });

            migrationBuilder.CreateTable(
                name: "TA_QUESTION_CATEGORIES",
                columns: table => new
                {
                    TA_QUESTION_CATEGORY_ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NAME = table.Column<string>(type: "VARCHAR(200)", nullable: false),
                    PARAM = table.Column<string>(type: "VARCHAR(200)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TA_QUESTION_CATEGORIES", x => x.TA_QUESTION_CATEGORY_ID);
                });

            migrationBuilder.CreateTable(
                name: "TA_QUESTION_DIFFICULTY",
                columns: table => new
                {
                    TA_QUESTION_DIFFICULTY_ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NAME = table.Column<string>(type: "VARCHAR(200)", nullable: false),
                    PARAM = table.Column<string>(type: "VARCHAR(200)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TA_QUESTION_DIFFICULTY", x => x.TA_QUESTION_DIFFICULTY_ID);
                });

            migrationBuilder.CreateTable(
                name: "USERS",
                columns: table => new
                {
                    USER_ID = table.Column<Guid>(type: "TEXT", nullable: false),
                    FULLNAME = table.Column<string>(type: "VARCHAR(100)", nullable: true),
                    DESCRIPTION = table.Column<string>(type: "VARCHAR(500)", nullable: true),
                    CREATED_AT = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UPDATED_AT = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ACTIVE = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: true),
                    USERNAME = table.Column<string>(type: "VARCHAR(100)", maxLength: 256, nullable: false),
                    USERNAME_NORMALIZED = table.Column<string>(type: "VARCHAR(100)", maxLength: 256, nullable: true),
                    EMAIL = table.Column<string>(type: "VARCHAT(256)", maxLength: 256, nullable: true),
                    EMAIL_NORMALIZED = table.Column<string>(type: "VARCHAR(256)", maxLength: 256, nullable: true),
                    PASSWORD_HASH = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USERS", x => x.USER_ID);
                });

            migrationBuilder.CreateTable(
                name: "MATCHES",
                columns: table => new
                {
                    MATCH_ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    GAME_ID = table.Column<int>(type: "INTEGER", nullable: false),
                    IS_ONGOING = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: true),
                    CREATED_AT = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UPDATED_AT = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ACTIVE = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MATCHES", x => x.MATCH_ID);
                    table.ForeignKey(
                        name: "FK_MATCHES_GAMES_GAME_ID",
                        column: x => x.GAME_ID,
                        principalTable: "GAMES",
                        principalColumn: "GAME_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CONNECTIONS",
                columns: table => new
                {
                    CONNECTION_ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    GROUP_ID = table.Column<int>(type: "INTEGER", nullable: false),
                    USER_ID = table.Column<Guid>(type: "TEXT", nullable: false),
                    USER_IDENTIFIER = table.Column<string>(type: "TEXT", nullable: true),
                    CREATED_AT = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UPDATED_AT = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ACTIVE = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CONNECTIONS", x => x.CONNECTION_ID);
                    table.ForeignKey(
                        name: "FK_CONNECTIONS_GROUPS_GROUP_ID",
                        column: x => x.GROUP_ID,
                        principalTable: "GROUPS",
                        principalColumn: "GROUP_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CONNECTIONS_USERS_USER_ID",
                        column: x => x.USER_ID,
                        principalTable: "USERS",
                        principalColumn: "USER_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "O2O_USER_ROLE",
                columns: table => new
                {
                    USER_ID = table.Column<Guid>(type: "TEXT", nullable: false),
                    ROLE_ID = table.Column<Guid>(type: "TEXT", nullable: false),
                    CREATED_AT = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UPDATED_AT = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ACTIVE = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_O2O_USER_ROLE", x => new { x.USER_ID, x.ROLE_ID });
                    table.ForeignKey(
                        name: "FK_O2O_USER_ROLE_ROLES_ROLE_ID",
                        column: x => x.ROLE_ID,
                        principalTable: "ROLES",
                        principalColumn: "ROLE_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_O2O_USER_ROLE_USERS_USER_ID",
                        column: x => x.USER_ID,
                        principalTable: "USERS",
                        principalColumn: "USER_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PLAYERS",
                columns: table => new
                {
                    PLAYER_ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    GAME_ID = table.Column<int>(type: "INTEGER", nullable: false),
                    USER_ID = table.Column<Guid>(type: "TEXT", nullable: false),
                    PLAYER_STATUS = table.Column<int>(type: "INTEGER", nullable: false),
                    REPLAY_STATUS = table.Column<int>(type: "INTEGER", nullable: true),
                    INVITE_STATUS = table.Column<int>(type: "INTEGER", nullable: true),
                    CREATED_AT = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UPDATED_AT = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ACTIVE = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PLAYERS", x => x.PLAYER_ID);
                    table.ForeignKey(
                        name: "FK_PLAYERS_GAMES_GAME_ID",
                        column: x => x.GAME_ID,
                        principalTable: "GAMES",
                        principalColumn: "GAME_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PLAYERS_USERS_USER_ID",
                        column: x => x.USER_ID,
                        principalTable: "USERS",
                        principalColumn: "USER_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QUESTIONS",
                columns: table => new
                {
                    QUESTION_ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ROW = table.Column<int>(type: "INTEGER", nullable: false),
                    GAME_ID = table.Column<int>(type: "INTEGER", nullable: false),
                    MATCH_ID = table.Column<int>(type: "INTEGER", nullable: false),
                    CATEGORY = table.Column<int>(type: "VARCHAR(200)", nullable: false),
                    Difficulty = table.Column<int>(type: "INTEGER", nullable: false),
                    SCORE_MULTIPLIER = table.Column<int>(type: "INTEGER", nullable: false),
                    DESCRIPTION = table.Column<string>(type: "VARCHAR(500)", nullable: false),
                    CORRECT_ANSWER = table.Column<string>(type: "VARCHAR(200)", nullable: false),
                    CORRECT_ANSWER_INDEX = table.Column<int>(type: "INTEGER", nullable: false),
                    REMAINING_TIME = table.Column<int>(type: "INTEGER", nullable: false),
                    ANSWER_CHOICES = table.Column<string>(type: "VARCHAR(500)", nullable: false),
                    IS_ONGOING = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: true),
                    CREATED_AT = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UPDATED_AT = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ACTIVE = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QUESTIONS", x => x.QUESTION_ID);
                    table.ForeignKey(
                        name: "FK_QUESTIONS_GAMES_GAME_ID",
                        column: x => x.GAME_ID,
                        principalTable: "GAMES",
                        principalColumn: "GAME_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QUESTIONS_MATCHES_MATCH_ID",
                        column: x => x.MATCH_ID,
                        principalTable: "MATCHES",
                        principalColumn: "MATCH_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RESULTS",
                columns: table => new
                {
                    RESULT_ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    GAME_ID = table.Column<int>(type: "INTEGER", nullable: false),
                    MATCH_ID = table.Column<int>(type: "INTEGER", nullable: false),
                    PLAYER_ID = table.Column<int>(type: "INTEGER", nullable: false),
                    TOTAL_SCORE = table.Column<int>(type: "INTEGER", nullable: false),
                    IS_WINNER = table.Column<bool>(type: "INTEGER", nullable: false),
                    NOTE = table.Column<string>(type: "TEXT", nullable: true),
                    CREATED_AT = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UPDATED_AT = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ACTIVE = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RESULTS", x => x.RESULT_ID);
                    table.ForeignKey(
                        name: "FK_RESULTS_GAMES_GAME_ID",
                        column: x => x.GAME_ID,
                        principalTable: "GAMES",
                        principalColumn: "GAME_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RESULTS_MATCHES_MATCH_ID",
                        column: x => x.MATCH_ID,
                        principalTable: "MATCHES",
                        principalColumn: "MATCH_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RESULTS_PLAYERS_PLAYER_ID",
                        column: x => x.PLAYER_ID,
                        principalTable: "PLAYERS",
                        principalColumn: "PLAYER_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ANSWERS",
                columns: table => new
                {
                    ANSWER_ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    GAME_ID = table.Column<int>(type: "INTEGER", nullable: false),
                    MATCH_ID = table.Column<int>(type: "INTEGER", nullable: false),
                    PLAYER_ID = table.Column<int>(type: "INTEGER", nullable: false),
                    QUESTION_ID = table.Column<int>(type: "INTEGER", nullable: false),
                    SELECTED_ANSWER = table.Column<string>(type: "TEXT", nullable: true),
                    SELECTED_ANSWER_INDEX = table.Column<int>(type: "INTEGER", nullable: true),
                    IS_CORRECT_ANSWER = table.Column<bool>(type: "INTEGER", nullable: false),
                    SCORE = table.Column<int>(type: "INTEGER", nullable: false),
                    CREATED_AT = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UPDATED_AT = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ACTIVE = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ANSWERS", x => x.ANSWER_ID);
                    table.ForeignKey(
                        name: "FK_ANSWERS_GAMES_GAME_ID",
                        column: x => x.GAME_ID,
                        principalTable: "GAMES",
                        principalColumn: "GAME_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ANSWERS_MATCHES_MATCH_ID",
                        column: x => x.MATCH_ID,
                        principalTable: "MATCHES",
                        principalColumn: "MATCH_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ANSWERS_PLAYERS_PLAYER_ID",
                        column: x => x.PLAYER_ID,
                        principalTable: "PLAYERS",
                        principalColumn: "PLAYER_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ANSWERS_QUESTIONS_QUESTION_ID",
                        column: x => x.QUESTION_ID,
                        principalTable: "QUESTIONS",
                        principalColumn: "QUESTION_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ANSWERS_GAME_ID",
                table: "ANSWERS",
                column: "GAME_ID");

            migrationBuilder.CreateIndex(
                name: "IX_ANSWERS_MATCH_ID",
                table: "ANSWERS",
                column: "MATCH_ID");

            migrationBuilder.CreateIndex(
                name: "IX_ANSWERS_PLAYER_ID",
                table: "ANSWERS",
                column: "PLAYER_ID");

            migrationBuilder.CreateIndex(
                name: "IX_ANSWERS_QUESTION_ID",
                table: "ANSWERS",
                column: "QUESTION_ID");

            migrationBuilder.CreateIndex(
                name: "IX_CONNECTIONS_GROUP_ID",
                table: "CONNECTIONS",
                column: "GROUP_ID");

            migrationBuilder.CreateIndex(
                name: "IX_CONNECTIONS_USER_ID",
                table: "CONNECTIONS",
                column: "USER_ID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GROUPS_GROUP_NAME",
                table: "GROUPS",
                column: "GROUP_NAME",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MATCHES_GAME_ID",
                table: "MATCHES",
                column: "GAME_ID");

            migrationBuilder.CreateIndex(
                name: "IX_O2O_USER_ROLE_ROLE_ID",
                table: "O2O_USER_ROLE",
                column: "ROLE_ID");

            migrationBuilder.CreateIndex(
                name: "IX_O2O_USER_ROLE_USER_ID",
                table: "O2O_USER_ROLE",
                column: "USER_ID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PLAYERS_GAME_ID",
                table: "PLAYERS",
                column: "GAME_ID");

            migrationBuilder.CreateIndex(
                name: "IX_PLAYERS_USER_ID",
                table: "PLAYERS",
                column: "USER_ID");

            migrationBuilder.CreateIndex(
                name: "IX_QUESTIONS_GAME_ID",
                table: "QUESTIONS",
                column: "GAME_ID");

            migrationBuilder.CreateIndex(
                name: "IX_QUESTIONS_MATCH_ID",
                table: "QUESTIONS",
                column: "MATCH_ID");

            migrationBuilder.CreateIndex(
                name: "IX_RESULTS_GAME_ID",
                table: "RESULTS",
                column: "GAME_ID");

            migrationBuilder.CreateIndex(
                name: "IX_RESULTS_MATCH_ID",
                table: "RESULTS",
                column: "MATCH_ID");

            migrationBuilder.CreateIndex(
                name: "IX_RESULTS_PLAYER_ID",
                table: "RESULTS",
                column: "PLAYER_ID");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "ROLES",
                column: "ROLE_NAME_NORMALIZED",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "USERS",
                column: "EMAIL_NORMALIZED");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "USERS",
                column: "USERNAME_NORMALIZED",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ANSWERS");

            migrationBuilder.DropTable(
                name: "CONNECTIONS");

            migrationBuilder.DropTable(
                name: "O2O_USER_ROLE");

            migrationBuilder.DropTable(
                name: "RESULTS");

            migrationBuilder.DropTable(
                name: "TA_QUESTION_CATEGORIES");

            migrationBuilder.DropTable(
                name: "TA_QUESTION_DIFFICULTY");

            migrationBuilder.DropTable(
                name: "QUESTIONS");

            migrationBuilder.DropTable(
                name: "GROUPS");

            migrationBuilder.DropTable(
                name: "ROLES");

            migrationBuilder.DropTable(
                name: "PLAYERS");

            migrationBuilder.DropTable(
                name: "MATCHES");

            migrationBuilder.DropTable(
                name: "USERS");

            migrationBuilder.DropTable(
                name: "GAMES");
        }
    }
}
