using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Okko : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Projects",
                columns: new[] { "Id", "OwnerId", "Title", "StartDate", "EndDate", "CreatedDate", "Description", "UpdateDate" },
                values: new object[]
                {
                   "8c56ac14-ae28-4425-9a19-690d27d3a16d",
                   "05e404b3-e235-4c11-bff4-3754b22c0245",
                   "Public Project",
                   new DateTime(2025, 5, 20, 1, 16, 18, 91, DateTimeKind.Utc).AddTicks(7210),
                   new DateTime(9999, 12, 31, 23, 59, 59, 999, DateTimeKind.Utc),
                   new DateTime(2025, 5, 20, 1, 16, 18, 91, DateTimeKind.Utc).AddTicks(7257),
                   "this is static project",
                   new DateTime(2025, 5, 20, 1, 16, 18, 91, DateTimeKind.Utc).AddTicks(7259)
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: "8c56ac14-ae28-4425-9a19-690d27d3a16d",
                columns: new[] { "CreatedDate", "StartDate", "UpdateDate" },
                values: new object[] { new DateTime(2025, 5, 20, 1, 8, 55, 27, DateTimeKind.Local).AddTicks(1845), new DateTime(2025, 5, 20, 1, 8, 55, 27, DateTimeKind.Local).AddTicks(1802), new DateTime(2025, 5, 20, 1, 8, 55, 27, DateTimeKind.Local).AddTicks(1847) });
        }
    }
}
