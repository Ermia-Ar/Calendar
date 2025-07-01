using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class intdd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSent",
                table: "Notifications",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: "8c56ac14-ae28-4425-9a19-690d27d3a16d",
                columns: new[] { "CreatedDate", "StartDate", "UpdateDate" },
                values: new object[] { new DateTime(2025, 7, 2, 1, 2, 53, 98, DateTimeKind.Local).AddTicks(9597), new DateTime(2025, 7, 2, 1, 2, 53, 98, DateTimeKind.Local).AddTicks(9551), new DateTime(2025, 7, 2, 1, 2, 53, 98, DateTimeKind.Local).AddTicks(9599) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSent",
                table: "Notifications");

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: "8c56ac14-ae28-4425-9a19-690d27d3a16d",
                columns: new[] { "CreatedDate", "StartDate", "UpdateDate" },
                values: new object[] { new DateTime(2025, 6, 29, 21, 55, 30, 967, DateTimeKind.Local).AddTicks(5235), new DateTime(2025, 6, 29, 21, 55, 30, 967, DateTimeKind.Local).AddTicks(5161), new DateTime(2025, 6, 29, 21, 55, 30, 967, DateTimeKind.Local).AddTicks(5239) });
        }
    }
}
