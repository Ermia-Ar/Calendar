using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Ololo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<TimeSpan>(
                name: "NotificationBefore",
                table: "Activities",
                type: "time",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: "8c56ac14-ae28-4425-9a19-690d27d3a16d",
                columns: new[] { "CreatedDate", "StartDate", "UpdateDate" },
                values: new object[] { new DateTime(2025, 5, 20, 1, 8, 55, 27, DateTimeKind.Local).AddTicks(1845), new DateTime(2025, 5, 20, 1, 8, 55, 27, DateTimeKind.Local).AddTicks(1802), new DateTime(2025, 5, 20, 1, 8, 55, 27, DateTimeKind.Local).AddTicks(1847) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NotificationBefore",
                table: "Activities");

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: "8c56ac14-ae28-4425-9a19-690d27d3a16d",
                columns: new[] { "CreatedDate", "StartDate", "UpdateDate" },
                values: new object[] { new DateTime(2025, 5, 19, 1, 4, 39, 807, DateTimeKind.Local).AddTicks(7496), new DateTime(2025, 5, 19, 1, 4, 39, 807, DateTimeKind.Local).AddTicks(7457), new DateTime(2025, 5, 19, 1, 4, 39, 807, DateTimeKind.Local).AddTicks(7498) });
        }
    }
}
