using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class No : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsReceiverSeen",
                table: "UserRequests",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSenderSeen",
                table: "UserRequests",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: "8c56ac14-ae28-4425-9a19-690d27d3a16d",
                columns: new[] { "CreatedDate", "StartDate", "UpdateDate" },
                values: new object[] { new DateTime(2025, 5, 9, 15, 13, 24, 966, DateTimeKind.Local).AddTicks(7810), new DateTime(2025, 5, 9, 15, 13, 24, 966, DateTimeKind.Local).AddTicks(7765), new DateTime(2025, 5, 9, 15, 13, 24, 966, DateTimeKind.Local).AddTicks(7812) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsReceiverSeen",
                table: "UserRequests");

            migrationBuilder.DropColumn(
                name: "IsSenderSeen",
                table: "UserRequests");

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: "8c56ac14-ae28-4425-9a19-690d27d3a16d",
                columns: new[] { "CreatedDate", "StartDate", "UpdateDate" },
                values: new object[] { new DateTime(2025, 5, 9, 3, 24, 26, 760, DateTimeKind.Local).AddTicks(1714), new DateTime(2025, 5, 9, 3, 24, 26, 760, DateTimeKind.Local).AddTicks(1675), new DateTime(2025, 5, 9, 3, 24, 26, 760, DateTimeKind.Local).AddTicks(1716) });
        }
    }
}
