using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class adrr : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: "8c56ac14-ae28-4425-9a19-690d27d3a16d",
                columns: new[] { "CreatedDate", "StartDate", "UpdateDate" },
                values: new object[] { new DateTime(2025, 6, 15, 21, 58, 46, 285, DateTimeKind.Local).AddTicks(7254), new DateTime(2025, 6, 15, 21, 58, 46, 285, DateTimeKind.Local).AddTicks(7206), new DateTime(2025, 6, 15, 21, 58, 46, 285, DateTimeKind.Local).AddTicks(7256) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: "8c56ac14-ae28-4425-9a19-690d27d3a16d",
                columns: new[] { "CreatedDate", "StartDate", "UpdateDate" },
                values: new object[] { new DateTime(2025, 6, 15, 21, 50, 16, 703, DateTimeKind.Local).AddTicks(7683), new DateTime(2025, 6, 15, 21, 50, 16, 703, DateTimeKind.Local).AddTicks(7638), new DateTime(2025, 6, 15, 21, 50, 16, 703, DateTimeKind.Local).AddTicks(7685) });
        }
    }
}
