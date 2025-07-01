using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Reand : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.EnsureSchema(
                name: "Calendar");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "Users",
                newSchema: "Calendar");

            migrationBuilder.RenameTable(
                name: "UserRoles",
                newName: "UserRoles",
                newSchema: "Calendar");

            migrationBuilder.RenameTable(
                name: "Roles",
                newName: "Roles",
                newSchema: "Calendar");

            migrationBuilder.RenameTable(
                name: "Requests",
                newName: "Requests",
                newSchema: "Calendar");

            migrationBuilder.RenameTable(
                name: "Projects",
                newName: "Projects",
                newSchema: "Calendar");

            migrationBuilder.RenameTable(
                name: "Notifications",
                newName: "Notifications",
                newSchema: "Calendar");

            migrationBuilder.RenameTable(
                name: "Comments",
                newName: "Comments",
                newSchema: "Calendar");

            migrationBuilder.RenameTable(
                name: "AspNetUserTokens",
                newName: "AspNetUserTokens",
                newSchema: "Calendar");

            migrationBuilder.RenameTable(
                name: "AspNetUserLogins",
                newName: "AspNetUserLogins",
                newSchema: "Calendar");

            migrationBuilder.RenameTable(
                name: "AspNetUserClaims",
                newName: "AspNetUserClaims",
                newSchema: "Calendar");

            migrationBuilder.RenameTable(
                name: "AspNetRoleClaims",
                newName: "AspNetRoleClaims",
                newSchema: "Calendar");

            migrationBuilder.RenameTable(
                name: "Activities",
                newName: "Activities",
                newSchema: "Calendar");

            migrationBuilder.RenameColumn(
                name: "UpdatedDate",
                schema: "Calendar",
                table: "Comments",
                newName: "UpdateDate");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                schema: "Calendar",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                schema: "Calendar",
                table: "Requests",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsArchived",
                schema: "Calendar",
                table: "Requests",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsEdited",
                schema: "Calendar",
                table: "Requests",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateDate",
                schema: "Calendar",
                table: "Requests",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                schema: "Calendar",
                table: "Projects",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                schema: "Calendar",
                table: "Notifications",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                schema: "Calendar",
                table: "Notifications",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsEdited",
                schema: "Calendar",
                table: "Notifications",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateDate",
                schema: "Calendar",
                table: "Notifications",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                schema: "Calendar",
                table: "Comments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                schema: "Calendar",
                table: "Activities",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                schema: "Calendar",
                table: "Projects",
                keyColumn: "Id",
                keyValue: "8c56ac14-ae28-4425-9a19-690d27d3a16d",
                columns: new[] { "CreatedDate", "IsActive", "StartDate", "UpdateDate" },
                values: new object[] { new DateTime(2025, 6, 29, 21, 37, 6, 303, DateTimeKind.Local).AddTicks(5379), false, new DateTime(2025, 6, 29, 21, 37, 6, 303, DateTimeKind.Local).AddTicks(5335), new DateTime(2025, 6, 29, 21, 37, 6, 303, DateTimeKind.Local).AddTicks(5381) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                schema: "Calendar",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                schema: "Calendar",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "IsArchived",
                schema: "Calendar",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "IsEdited",
                schema: "Calendar",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "UpdateDate",
                schema: "Calendar",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "IsActive",
                schema: "Calendar",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                schema: "Calendar",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "IsActive",
                schema: "Calendar",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "IsEdited",
                schema: "Calendar",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "UpdateDate",
                schema: "Calendar",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "IsActive",
                schema: "Calendar",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "IsActive",
                schema: "Calendar",
                table: "Activities");

            migrationBuilder.RenameTable(
                name: "Users",
                schema: "Calendar",
                newName: "Users");

            migrationBuilder.RenameTable(
                name: "UserRoles",
                schema: "Calendar",
                newName: "UserRoles");

            migrationBuilder.RenameTable(
                name: "Roles",
                schema: "Calendar",
                newName: "Roles");

            migrationBuilder.RenameTable(
                name: "Requests",
                schema: "Calendar",
                newName: "Requests");

            migrationBuilder.RenameTable(
                name: "Projects",
                schema: "Calendar",
                newName: "Projects");

            migrationBuilder.RenameTable(
                name: "Notifications",
                schema: "Calendar",
                newName: "Notifications");

            migrationBuilder.RenameTable(
                name: "Comments",
                schema: "Calendar",
                newName: "Comments");

            migrationBuilder.RenameTable(
                name: "AspNetUserTokens",
                schema: "Calendar",
                newName: "AspNetUserTokens");

            migrationBuilder.RenameTable(
                name: "AspNetUserLogins",
                schema: "Calendar",
                newName: "AspNetUserLogins");

            migrationBuilder.RenameTable(
                name: "AspNetUserClaims",
                schema: "Calendar",
                newName: "AspNetUserClaims");

            migrationBuilder.RenameTable(
                name: "AspNetRoleClaims",
                schema: "Calendar",
                newName: "AspNetRoleClaims");

            migrationBuilder.RenameTable(
                name: "Activities",
                schema: "Calendar",
                newName: "Activities");

            migrationBuilder.RenameColumn(
                name: "UpdateDate",
                table: "Comments",
                newName: "UpdatedDate");

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
    }
}
