using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddReviewFeature : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LearningSessions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", maxLength: 12, nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 24, nullable: false),
                    UserId = table.Column<string>(type: "TEXT", maxLength: 12, nullable: false),
                    DeckId = table.Column<string>(type: "TEXT", maxLength: 12, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LearningSessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LearningSessions_Decks_DeckId",
                        column: x => x.DeckId,
                        principalTable: "Decks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LearningSessions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", maxLength: 12, nullable: false),
                    LearningSessionId = table.Column<string>(type: "TEXT", maxLength: 12, nullable: false),
                    FlashcardId = table.Column<string>(type: "TEXT", maxLength: 12, nullable: false),
                    ClozeIndex = table.Column<int>(type: "INTEGER", nullable: false),
                    DueDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Stability = table.Column<float>(type: "REAL", nullable: false),
                    Difficulty = table.Column<float>(type: "REAL", nullable: false),
                    ElapsedDays = table.Column<int>(type: "INTEGER", nullable: false),
                    ScheduledDays = table.Column<int>(type: "INTEGER", nullable: false),
                    Reps = table.Column<int>(type: "INTEGER", nullable: false),
                    Lapses = table.Column<int>(type: "INTEGER", nullable: false),
                    State = table.Column<int>(type: "INTEGER", nullable: false),
                    LastReviewDate = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reviews_Flashcards_FlashcardId",
                        column: x => x.FlashcardId,
                        principalTable: "Flashcards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reviews_LearningSessions_LearningSessionId",
                        column: x => x.LearningSessionId,
                        principalTable: "LearningSessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReviewLogs",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", maxLength: 12, nullable: false),
                    LearningSessionId = table.Column<string>(type: "TEXT", maxLength: 12, nullable: false),
                    ReviewId = table.Column<string>(type: "TEXT", maxLength: 12, nullable: false),
                    Rating = table.Column<int>(type: "INTEGER", nullable: false),
                    State = table.Column<int>(type: "INTEGER", nullable: false),
                    DueDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Stability = table.Column<float>(type: "REAL", nullable: false),
                    Difficulty = table.Column<float>(type: "REAL", nullable: false),
                    ElapsedDays = table.Column<int>(type: "INTEGER", nullable: false),
                    LastElapsedDays = table.Column<int>(type: "INTEGER", nullable: false),
                    ScheduledDays = table.Column<int>(type: "INTEGER", nullable: false),
                    ReviewDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReviewLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReviewLogs_LearningSessions_LearningSessionId",
                        column: x => x.LearningSessionId,
                        principalTable: "LearningSessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReviewLogs_LearningSessions_ReviewId",
                        column: x => x.ReviewId,
                        principalTable: "LearningSessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReviewLogs_Reviews_ReviewId",
                        column: x => x.ReviewId,
                        principalTable: "Reviews",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LearningSessions_DeckId",
                table: "LearningSessions",
                column: "DeckId");

            migrationBuilder.CreateIndex(
                name: "IX_LearningSessions_UserId",
                table: "LearningSessions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ReviewLogs_LearningSessionId",
                table: "ReviewLogs",
                column: "LearningSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_ReviewLogs_ReviewId",
                table: "ReviewLogs",
                column: "ReviewId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_FlashcardId",
                table: "Reviews",
                column: "FlashcardId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_LearningSessionId",
                table: "Reviews",
                column: "LearningSessionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReviewLogs");

            migrationBuilder.DropTable(
                name: "Reviews");

            migrationBuilder.DropTable(
                name: "LearningSessions");
        }
    }
}
