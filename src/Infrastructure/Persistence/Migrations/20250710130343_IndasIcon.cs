using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class IndasIcon : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActivityMembers_Notifications_ActivityId",
                schema: "Calendar",
                table: "ActivityMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_Requests_Notifications_NotificationId",
                schema: "Calendar",
                table: "Requests");

            migrationBuilder.DropIndex(
                name: "IX_Requests_NotificationId",
                schema: "Calendar",
                table: "Requests");

            migrationBuilder.DropIndex(
                name: "IX_ActivityMembers_ActivityId",
                schema: "Calendar",
                table: "ActivityMembers");

            migrationBuilder.DropColumn(
                name: "NotificationId",
                schema: "Calendar",
                table: "Requests");

            migrationBuilder.AddColumn<string>(
                name: "Color",
                schema: "Calendar",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Icon",
                schema: "Calendar",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_ActivityMemberId",
                schema: "Calendar",
                table: "Notifications",
                column: "ActivityMemberId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ActivityMembers_ActivityId",
                schema: "Calendar",
                table: "ActivityMembers",
                column: "ActivityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_ActivityMembers_ActivityMemberId",
                schema: "Calendar",
                table: "Notifications",
                column: "ActivityMemberId",
                principalSchema: "Calendar",
                principalTable: "ActivityMembers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_ActivityMembers_ActivityMemberId",
                schema: "Calendar",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_ActivityMemberId",
                schema: "Calendar",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_ActivityMembers_ActivityId",
                schema: "Calendar",
                table: "ActivityMembers");

            migrationBuilder.DropColumn(
                name: "Color",
                schema: "Calendar",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "Icon",
                schema: "Calendar",
                table: "Projects");

            migrationBuilder.AddColumn<long>(
                name: "NotificationId",
                schema: "Calendar",
                table: "Requests",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Requests_NotificationId",
                schema: "Calendar",
                table: "Requests",
                column: "NotificationId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityMembers_ActivityId",
                schema: "Calendar",
                table: "ActivityMembers",
                column: "ActivityId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityMembers_Notifications_ActivityId",
                schema: "Calendar",
                table: "ActivityMembers",
                column: "ActivityId",
                principalSchema: "Calendar",
                principalTable: "Notifications",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_Notifications_NotificationId",
                schema: "Calendar",
                table: "Requests",
                column: "NotificationId",
                principalSchema: "Calendar",
                principalTable: "Notifications",
                principalColumn: "Id");
        }
    }
}
