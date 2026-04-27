using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ErpShowroom.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddJournalStatusAndColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "PostedAt",
                schema: "acc",
                table: "JournalEntry",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "PostedByUserId",
                schema: "acc",
                table: "JournalEntry",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                schema: "acc",
                table: "JournalEntry",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PostedAt",
                schema: "acc",
                table: "JournalEntry");

            migrationBuilder.DropColumn(
                name: "PostedByUserId",
                schema: "acc",
                table: "JournalEntry");

            migrationBuilder.DropColumn(
                name: "Status",
                schema: "acc",
                table: "JournalEntry");
        }
    }
}
