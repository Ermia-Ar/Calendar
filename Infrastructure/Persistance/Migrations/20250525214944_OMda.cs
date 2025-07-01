using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class OMda : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activities_Users_UserId",
                table: "Activities");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Users_OwnerId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "Receiver",
                table: "UserRequests");

            migrationBuilder.DropColumn(
                name: "Sender",
                table: "UserRequests");

            migrationBuilder.AddColumn<string>(
                name: "ReceiverId",
                table: "UserRequests",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SenderId",
                table: "UserRequests",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: "8c56ac14-ae28-4425-9a19-690d27d3a16d",
                columns: new[] { "CreatedDate", "StartDate", "UpdateDate" },
                values: new object[] { new DateTime(2025, 5, 26, 2, 19, 44, 184, DateTimeKind.Local).AddTicks(2309), new DateTime(2025, 5, 26, 2, 19, 44, 184, DateTimeKind.Local).AddTicks(2266), new DateTime(2025, 5, 26, 2, 19, 44, 184, DateTimeKind.Local).AddTicks(2311) });

            migrationBuilder.CreateIndex(
                name: "IX_UserRequests_ReceiverId",
                table: "UserRequests",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRequests_SenderId",
                table: "UserRequests",
                column: "SenderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_Users_UserId",
                table: "Activities",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Users_OwnerId",
                table: "Projects",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserRequests_Users_ReceiverId",
                table: "UserRequests",
                column: "ReceiverId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserRequests_Users_SenderId",
                table: "UserRequests",
                column: "SenderId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activities_Users_UserId",
                table: "Activities");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Users_OwnerId",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRequests_Users_ReceiverId",
                table: "UserRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRequests_Users_SenderId",
                table: "UserRequests");

            migrationBuilder.DropIndex(
                name: "IX_UserRequests_ReceiverId",
                table: "UserRequests");

            migrationBuilder.DropIndex(
                name: "IX_UserRequests_SenderId",
                table: "UserRequests");

            migrationBuilder.DropColumn(
                name: "ReceiverId",
                table: "UserRequests");

            migrationBuilder.DropColumn(
                name: "SenderId",
                table: "UserRequests");

            migrationBuilder.AddColumn<string>(
                name: "Receiver",
                table: "UserRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Sender",
                table: "UserRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: "8c56ac14-ae28-4425-9a19-690d27d3a16d",
                columns: new[] { "CreatedDate", "StartDate", "UpdateDate" },
                values: new object[] { new DateTime(2025, 5, 20, 1, 22, 1, 636, DateTimeKind.Local).AddTicks(4763), new DateTime(2025, 5, 20, 1, 22, 1, 636, DateTimeKind.Local).AddTicks(4720), new DateTime(2025, 5, 20, 1, 22, 1, 636, DateTimeKind.Local).AddTicks(4765) });

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_Users_UserId",
                table: "Activities",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Users_OwnerId",
                table: "Projects",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
