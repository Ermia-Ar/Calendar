using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class New : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ActivityId",
                table: "Notifications",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: "8c56ac14-ae28-4425-9a19-690d27d3a16d",
                columns: new[] { "CreatedDate", "StartDate", "UpdateDate" },
                values: new object[] { new DateTime(2025, 6, 22, 19, 9, 9, 743, DateTimeKind.Local).AddTicks(6739), new DateTime(2025, 6, 22, 19, 9, 9, 743, DateTimeKind.Local).AddTicks(6687), new DateTime(2025, 6, 22, 19, 9, 9, 743, DateTimeKind.Local).AddTicks(6741) });

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_ActivityId",
                table: "Notifications",
                column: "ActivityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Activities_ActivityId",
                table: "Notifications",
                column: "ActivityId",
                principalTable: "Activities",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Activities_ActivityId",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_ActivityId",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "ActivityId",
                table: "Notifications");

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: "8c56ac14-ae28-4425-9a19-690d27d3a16d",
                columns: new[] { "CreatedDate", "StartDate", "UpdateDate" },
                values: new object[] { new DateTime(2025, 6, 22, 16, 13, 46, 750, DateTimeKind.Local).AddTicks(8998), new DateTime(2025, 6, 22, 16, 13, 46, 750, DateTimeKind.Local).AddTicks(8953), new DateTime(2025, 6, 22, 16, 13, 46, 750, DateTimeKind.Local).AddTicks(9000) });
        }
    }
}
