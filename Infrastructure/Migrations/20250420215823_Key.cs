using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Key : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActivityGuests_Activities_ActivityId",
                table: "ActivityGuests");

            migrationBuilder.DropIndex(
                name: "IX_ActivityGuests_ActivityId",
                table: "ActivityGuests");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ActivityGuests",
                table: "ActivityGuests",
                columns: new[] { "ActivityId", "UserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityGuests_Activities_ActivityId",
                table: "ActivityGuests",
                column: "ActivityId",
                principalTable: "Activities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActivityGuests_Activities_ActivityId",
                table: "ActivityGuests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ActivityGuests",
                table: "ActivityGuests");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityGuests_ActivityId",
                table: "ActivityGuests",
                column: "ActivityId");

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityGuests_Activities_ActivityId",
                table: "ActivityGuests",
                column: "ActivityId",
                principalTable: "Activities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
