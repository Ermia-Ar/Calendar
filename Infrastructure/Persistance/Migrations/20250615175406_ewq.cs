using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ewq : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activities_User_UserId",
                table: "Activities");

            migrationBuilder.DropForeignKey(
                name: "FK_Activities_Users_UserId1",
                table: "Activities");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_User_UserId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Users_UserId1",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_User_OwnerId",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Users_UserId",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRequests_User_ReceiverId",
                table: "UserRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRequests_User_SenderId",
                table: "UserRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRequests_Users_UserId",
                table: "UserRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRequests_Users_UserId1",
                table: "UserRequests");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropIndex(
                name: "IX_UserRequests_UserId",
                table: "UserRequests");

            migrationBuilder.DropIndex(
                name: "IX_UserRequests_UserId1",
                table: "UserRequests");

            migrationBuilder.DropIndex(
                name: "IX_Projects_UserId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Comments_UserId1",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Activities_UserId1",
                table: "Activities");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "UserRequests");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "UserRequests");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "Activities");

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: "8c56ac14-ae28-4425-9a19-690d27d3a16d",
                columns: new[] { "CreatedDate", "StartDate", "UpdateDate" },
                values: new object[] { new DateTime(2025, 6, 15, 22, 24, 5, 937, DateTimeKind.Local).AddTicks(1677), new DateTime(2025, 6, 15, 22, 24, 5, 937, DateTimeKind.Local).AddTicks(1631), new DateTime(2025, 6, 15, 22, 24, 5, 937, DateTimeKind.Local).AddTicks(1679) });

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_Users_UserId",
                table: "Activities",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Users_UserId",
                table: "Comments",
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
                name: "FK_Comments_Users_UserId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Users_OwnerId",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRequests_Users_ReceiverId",
                table: "UserRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRequests_Users_SenderId",
                table: "UserRequests");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "UserRequests",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "UserRequests",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Projects",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "Comments",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "Activities",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Category = table.Column<int>(type: "int", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: "8c56ac14-ae28-4425-9a19-690d27d3a16d",
                columns: new[] { "CreatedDate", "StartDate", "UpdateDate", "UserId" },
                values: new object[] { new DateTime(2025, 6, 15, 21, 58, 46, 285, DateTimeKind.Local).AddTicks(7254), new DateTime(2025, 6, 15, 21, 58, 46, 285, DateTimeKind.Local).AddTicks(7206), new DateTime(2025, 6, 15, 21, 58, 46, 285, DateTimeKind.Local).AddTicks(7256), null });

            migrationBuilder.CreateIndex(
                name: "IX_UserRequests_UserId",
                table: "UserRequests",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRequests_UserId1",
                table: "UserRequests",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_UserId",
                table: "Projects",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_UserId1",
                table: "Comments",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_Activities_UserId1",
                table: "Activities",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_User_UserId",
                table: "Activities",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_Users_UserId1",
                table: "Activities",
                column: "UserId1",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_User_UserId",
                table: "Comments",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Users_UserId1",
                table: "Comments",
                column: "UserId1",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_User_OwnerId",
                table: "Projects",
                column: "OwnerId",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Users_UserId",
                table: "Projects",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserRequests_User_ReceiverId",
                table: "UserRequests",
                column: "ReceiverId",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserRequests_User_SenderId",
                table: "UserRequests",
                column: "SenderId",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserRequests_Users_UserId",
                table: "UserRequests",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserRequests_Users_UserId1",
                table: "UserRequests",
                column: "UserId1",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
