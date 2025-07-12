using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDefaultSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Calendar");

            migrationBuilder.RenameTable(
                name: "Requests",
                newName: "Requests",
                newSchema: "Calendar");

            migrationBuilder.RenameTable(
                name: "Projects",
                newName: "Projects",
                newSchema: "Calendar");

            migrationBuilder.RenameTable(
                name: "ProjectMembers",
                newName: "ProjectMembers",
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
                name: "ActivityMembers",
                newName: "ActivityMembers",
                newSchema: "Calendar");

            migrationBuilder.RenameTable(
                name: "Activities",
                newName: "Activities",
                newSchema: "Calendar");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "Requests",
                schema: "Calendar",
                newName: "Requests");

            migrationBuilder.RenameTable(
                name: "Projects",
                schema: "Calendar",
                newName: "Projects");

            migrationBuilder.RenameTable(
                name: "ProjectMembers",
                schema: "Calendar",
                newName: "ProjectMembers");

            migrationBuilder.RenameTable(
                name: "Notifications",
                schema: "Calendar",
                newName: "Notifications");

            migrationBuilder.RenameTable(
                name: "Comments",
                schema: "Calendar",
                newName: "Comments");

            migrationBuilder.RenameTable(
                name: "ActivityMembers",
                schema: "Calendar",
                newName: "ActivityMembers");

            migrationBuilder.RenameTable(
                name: "Activities",
                schema: "Calendar",
                newName: "Activities");
        }
    }
}
