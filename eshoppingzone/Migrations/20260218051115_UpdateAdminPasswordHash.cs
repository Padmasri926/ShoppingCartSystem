using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eshoppingzone.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAdminPasswordHash : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin-seed-id",
                column: "PasswordHash",
                value: "AQAAAAEAACcQAAAAEI9y1k6DnupfaNmgPsFGu1I1OTCxJdPZj4hssewGeJygWn3pHyCcyXAbQtOcPHLCdw==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin-seed-id",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEJ8fHQqLxvbqJZ8fHQqLxvbqJZ8fHQqLxvbqJZ8fHQqLxvbqJZ8fHQqLxvbqJQ==");
        }
    }
}
