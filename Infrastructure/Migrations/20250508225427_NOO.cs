using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class NOO : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activities_Projects_ProjectId",
                table: "Activities");

            migrationBuilder.DropTable(
                name: "ProjectUsers");

            migrationBuilder.AlterColumn<string>(
                name: "ProjectId",
                table: "Activities",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "Projects",
                columns: new[] { "Id", "CreatedDate", "Description", "EndDate", "OwnerId", "StartDate", "Title", "UpdateDate" },
                values: new object[] { "8c56ac14-ae28-4425-9a19-690d27d3a16d", new DateTime(2025, 5, 9, 3, 24, 26, 760, DateTimeKind.Local).AddTicks(1714), "this is static project", new DateTime(9999, 12, 31, 23, 59, 59, 999, DateTimeKind.Unspecified).AddTicks(9999), "05e404b3-e235-4c11-bff4-3754b22c0245", new DateTime(2025, 5, 9, 3, 24, 26, 760, DateTimeKind.Local).AddTicks(1675), "Public Project", new DateTime(2025, 5, 9, 3, 24, 26, 760, DateTimeKind.Local).AddTicks(1716) });

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_Projects_ProjectId",
                table: "Activities",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activities_Projects_ProjectId",
                table: "Activities");

            migrationBuilder.DeleteData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: "8c56ac14-ae28-4425-9a19-690d27d3a16d");

            migrationBuilder.AlterColumn<string>(
                name: "ProjectId",
                table: "Activities",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateTable(
                name: "ProjectUsers",
                columns: table => new
                {
                    MembersId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProjectsId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectUsers", x => new { x.MembersId, x.ProjectsId });
                    table.ForeignKey(
                        name: "FK_ProjectUsers_Projects_ProjectsId",
                        column: x => x.ProjectsId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectUsers_Users_MembersId",
                        column: x => x.MembersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectUsers_ProjectsId",
                table: "ProjectUsers",
                column: "ProjectsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_Projects_ProjectId",
                table: "Activities",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id");
        }
    }
}
