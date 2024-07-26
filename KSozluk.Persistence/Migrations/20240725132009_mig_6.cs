using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KSozluk.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class mig_6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AcceptorId",
                table: "words",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "RecommenderId",
                table: "words",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "lastediteddate",
                table: "words",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<int>(
                name: "status",
                table: "descriptions",
                type: "integer",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AddColumn<Guid>(
                name: "AcceptorId",
                table: "descriptions",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "RecommenderId",
                table: "descriptions",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "lastediteddate",
                table: "descriptions",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_words_AcceptorId",
                table: "words",
                column: "AcceptorId");

            migrationBuilder.CreateIndex(
                name: "IX_words_RecommenderId",
                table: "words",
                column: "RecommenderId");

            migrationBuilder.CreateIndex(
                name: "IX_descriptions_AcceptorId",
                table: "descriptions",
                column: "AcceptorId");

            migrationBuilder.CreateIndex(
                name: "IX_descriptions_RecommenderId",
                table: "descriptions",
                column: "RecommenderId");

            migrationBuilder.AddForeignKey(
                name: "FK_descriptions_users_AcceptorId",
                table: "descriptions",
                column: "AcceptorId",
                principalTable: "users",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_descriptions_users_RecommenderId",
                table: "descriptions",
                column: "RecommenderId",
                principalTable: "users",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_words_users_AcceptorId",
                table: "words",
                column: "AcceptorId",
                principalTable: "users",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_words_users_RecommenderId",
                table: "words",
                column: "RecommenderId",
                principalTable: "users",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_descriptions_users_AcceptorId",
                table: "descriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_descriptions_users_RecommenderId",
                table: "descriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_words_users_AcceptorId",
                table: "words");

            migrationBuilder.DropForeignKey(
                name: "FK_words_users_RecommenderId",
                table: "words");

            migrationBuilder.DropIndex(
                name: "IX_words_AcceptorId",
                table: "words");

            migrationBuilder.DropIndex(
                name: "IX_words_RecommenderId",
                table: "words");

            migrationBuilder.DropIndex(
                name: "IX_descriptions_AcceptorId",
                table: "descriptions");

            migrationBuilder.DropIndex(
                name: "IX_descriptions_RecommenderId",
                table: "descriptions");

            migrationBuilder.DropColumn(
                name: "AcceptorId",
                table: "words");

            migrationBuilder.DropColumn(
                name: "RecommenderId",
                table: "words");

            migrationBuilder.DropColumn(
                name: "lastediteddate",
                table: "words");

            migrationBuilder.DropColumn(
                name: "AcceptorId",
                table: "descriptions");

            migrationBuilder.DropColumn(
                name: "RecommenderId",
                table: "descriptions");

            migrationBuilder.DropColumn(
                name: "lastediteddate",
                table: "descriptions");

            migrationBuilder.AlterColumn<bool>(
                name: "status",
                table: "descriptions",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");
        }
    }
}
