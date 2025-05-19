using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Olala : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsEdited",
                table: "Activities",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ParentId",
                table: "Activities",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: "8c56ac14-ae28-4425-9a19-690d27d3a16d",
                columns: new[] { "CreatedDate", "StartDate", "UpdateDate" },
                values: new object[] { new DateTime(2025, 5, 19, 1, 4, 39, 807, DateTimeKind.Local).AddTicks(7496), new DateTime(2025, 5, 19, 1, 4, 39, 807, DateTimeKind.Local).AddTicks(7457), new DateTime(2025, 5, 19, 1, 4, 39, 807, DateTimeKind.Local).AddTicks(7498) });

            migrationBuilder.CreateIndex(
                name: "IX_Activities_ParentId",
                table: "Activities",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_Activities_ParentId",
                table: "Activities",
                column: "ParentId",
                principalTable: "Activities",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activities_Activities_ParentId",
                table: "Activities");

            migrationBuilder.DropIndex(
                name: "IX_Activities_ParentId",
                table: "Activities");

            migrationBuilder.DropColumn(
                name: "IsEdited",
                table: "Activities");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "Activities");

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: "8c56ac14-ae28-4425-9a19-690d27d3a16d",
                columns: new[] { "CreatedDate", "StartDate", "UpdateDate" },
                values: new object[] { new DateTime(2025, 5, 19, 0, 23, 36, 746, DateTimeKind.Local).AddTicks(245), new DateTime(2025, 5, 19, 0, 23, 36, 746, DateTimeKind.Local).AddTicks(204), new DateTime(2025, 5, 19, 0, 23, 36, 746, DateTimeKind.Local).AddTicks(247) });
        }
    }
}
