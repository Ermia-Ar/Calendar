using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddProjectData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: "8c56ac14-ae28-4425-9a19-690d27d3a16d",
                columns: new[] { "CreatedDate", "StartDate", "UpdateDate" },
                values: new object[] { new DateTime(2025, 5, 9, 15, 18, 14, 534, DateTimeKind.Local).AddTicks(1711), new DateTime(2025, 5, 9, 15, 18, 14, 534, DateTimeKind.Local).AddTicks(1673), new DateTime(2025, 5, 9, 15, 18, 14, 534, DateTimeKind.Local).AddTicks(1713) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: "8c56ac14-ae28-4425-9a19-690d27d3a16d",
                columns: new[] { "CreatedDate", "StartDate", "UpdateDate" },
                values: new object[] { new DateTime(2025, 5, 9, 15, 13, 24, 966, DateTimeKind.Local).AddTicks(7810), new DateTime(2025, 5, 9, 15, 13, 24, 966, DateTimeKind.Local).AddTicks(7765), new DateTime(2025, 5, 9, 15, 13, 24, 966, DateTimeKind.Local).AddTicks(7812) });
        }
    }
}
