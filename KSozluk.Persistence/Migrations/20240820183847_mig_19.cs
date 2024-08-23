using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KSozluk.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class mig_19 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_descriptions_previusdescid",
                table: "descriptions",
                column: "previusdescid");

            migrationBuilder.AddForeignKey(
                name: "FK_descriptions_descriptions_previusdescid",
                table: "descriptions",
                column: "previusdescid",
                principalTable: "descriptions",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_descriptions_descriptions_previusdescid",
                table: "descriptions");

            migrationBuilder.DropIndex(
                name: "IX_descriptions_previusdescid",
                table: "descriptions");
        }
    }
}
