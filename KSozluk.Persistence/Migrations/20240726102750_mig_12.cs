using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KSozluk.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class mig_12 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_descriptions_words_WordId1",
                table: "descriptions");

            migrationBuilder.DropIndex(
                name: "IX_descriptions_WordId1",
                table: "descriptions");

            migrationBuilder.DropColumn(
                name: "WordId1",
                table: "descriptions");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "WordId1",
                table: "descriptions",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_descriptions_WordId1",
                table: "descriptions",
                column: "WordId1");

            migrationBuilder.AddForeignKey(
                name: "FK_descriptions_words_WordId1",
                table: "descriptions",
                column: "WordId1",
                principalTable: "words",
                principalColumn: "id");
        }
    }
}
