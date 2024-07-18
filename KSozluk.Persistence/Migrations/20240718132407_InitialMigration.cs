using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KSozluk.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    fullname = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    password = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    permissions = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "words",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    word = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    status = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_words", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "descriptions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    description = table.Column<string>(type: "character varying(550)", maxLength: 550, nullable: false),
                    WordId = table.Column<Guid>(type: "uuid", nullable: true),
                    order = table.Column<double>(type: "double precision", nullable: false),
                    status = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_descriptions", x => x.id);
                    table.ForeignKey(
                        name: "FK_descriptions_words_WordId",
                        column: x => x.WordId,
                        principalTable: "words",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_descriptions_WordId",
                table: "descriptions",
                column: "WordId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "descriptions");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "words");
        }
    }
}
