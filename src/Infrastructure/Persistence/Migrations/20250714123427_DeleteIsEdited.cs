using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DeleteIsEdited : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsEdited",
                schema: "Calendar",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "IsEdited",
                schema: "Calendar",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "IsEdited",
                schema: "Calendar",
                table: "ProjectMembers");

            migrationBuilder.DropColumn(
                name: "IsEdited",
                schema: "Calendar",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "IsEdited",
                schema: "Calendar",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "IsEdited",
                schema: "Calendar",
                table: "ActivityMembers");

            migrationBuilder.DropColumn(
                name: "IsEdited",
                schema: "Calendar",
                table: "Activities");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsEdited",
                schema: "Calendar",
                table: "Requests",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsEdited",
                schema: "Calendar",
                table: "Projects",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsEdited",
                schema: "Calendar",
                table: "ProjectMembers",
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

            migrationBuilder.AddColumn<bool>(
                name: "IsEdited",
                schema: "Calendar",
                table: "Comments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsEdited",
                schema: "Calendar",
                table: "ActivityMembers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsEdited",
                schema: "Calendar",
                table: "Activities",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
