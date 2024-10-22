using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KSozluk.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class deneme3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "description_like",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    description_id = table.Column<Guid>(type: "uuid", nullable: false),
                    timestamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_description_like", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "favorite_word",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    word_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_favorite_word", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    fullname = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    password = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    permissions = table.Column<short>(type: "smallint", nullable: false),
                    refreshtoken = table.Column<string>(type: "character varying(55)", maxLength: 55, nullable: true),
                    tokenexpiredate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "word_like",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    word_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    timestamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_word_like", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "words",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    word = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    AcceptorId = table.Column<Guid>(type: "uuid", nullable: true),
                    RecommenderId = table.Column<Guid>(type: "uuid", nullable: true),
                    lastediteddate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_words", x => x.id);
                    table.ForeignKey(
                        name: "FK_words_users_AcceptorId",
                        column: x => x.AcceptorId,
                        principalTable: "users",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_words_users_RecommenderId",
                        column: x => x.RecommenderId,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "descriptions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    previusdescid = table.Column<Guid>(type: "uuid", nullable: true),
                    description = table.Column<string>(type: "character varying(550)", maxLength: 550, nullable: false),
                    WordId = table.Column<Guid>(type: "uuid", nullable: false),
                    order = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    AcceptorId = table.Column<Guid>(type: "uuid", nullable: true),
                    RecommenderId = table.Column<Guid>(type: "uuid", nullable: true),
                    lastediteddate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_descriptions", x => x.id);
                    table.ForeignKey(
                        name: "FK_descriptions_descriptions_previusdescid",
                        column: x => x.previusdescid,
                        principalTable: "descriptions",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_descriptions_users_AcceptorId",
                        column: x => x.AcceptorId,
                        principalTable: "users",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_descriptions_users_RecommenderId",
                        column: x => x.RecommenderId,
                        principalTable: "users",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_descriptions_words_WordId",
                        column: x => x.WordId,
                        principalTable: "words",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_descriptions_AcceptorId",
                table: "descriptions",
                column: "AcceptorId");

            migrationBuilder.CreateIndex(
                name: "IX_descriptions_previusdescid",
                table: "descriptions",
                column: "previusdescid");

            migrationBuilder.CreateIndex(
                name: "IX_descriptions_RecommenderId",
                table: "descriptions",
                column: "RecommenderId");

            migrationBuilder.CreateIndex(
                name: "IX_descriptions_WordId",
                table: "descriptions",
                column: "WordId");

            migrationBuilder.CreateIndex(
                name: "IX_words_AcceptorId",
                table: "words",
                column: "AcceptorId");

            migrationBuilder.CreateIndex(
                name: "IX_words_RecommenderId",
                table: "words",
                column: "RecommenderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "description_like");

            migrationBuilder.DropTable(
                name: "descriptions");

            migrationBuilder.DropTable(
                name: "favorite_word");

            migrationBuilder.DropTable(
                name: "word_like");

            migrationBuilder.DropTable(
                name: "words");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
