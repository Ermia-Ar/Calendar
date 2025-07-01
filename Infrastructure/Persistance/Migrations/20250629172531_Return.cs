using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Return : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: "8c56ac14-ae28-4425-9a19-690d27d3a16d",
                columns: new[] { "CreatedDate", "StartDate", "UpdateDate" },
                values: new object[] { new DateTime(2025, 6, 29, 21, 55, 30, 967, DateTimeKind.Local).AddTicks(5235), new DateTime(2025, 6, 29, 21, 55, 30, 967, DateTimeKind.Local).AddTicks(5161), new DateTime(2025, 6, 29, 21, 55, 30, 967, DateTimeKind.Local).AddTicks(5239) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.UpdateData(
                schema: "Calendar",
                table: "Projects",
                keyColumn: "Id",
                keyValue: "8c56ac14-ae28-4425-9a19-690d27d3a16d",
                columns: new[] { "CreatedDate", "StartDate", "UpdateDate" },
                values: new object[] { new DateTime(2025, 6, 29, 21, 37, 6, 303, DateTimeKind.Local).AddTicks(5379), new DateTime(2025, 6, 29, 21, 37, 6, 303, DateTimeKind.Local).AddTicks(5335), new DateTime(2025, 6, 29, 21, 37, 6, 303, DateTimeKind.Local).AddTicks(5381) });
        }
    }
}
