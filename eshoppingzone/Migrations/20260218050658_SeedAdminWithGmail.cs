using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eshoppingzone.Migrations
{
    /// <inheritdoc />
    public partial class SeedAdminWithGmail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin-seed-id",
                columns: new[] { "Email", "NormalizedEmail", "NormalizedUserName", "UserName" },
                values: new object[] { "admin@gmail.com", "ADMIN@GMAIL.COM", "ADMIN@GMAIL.COM", "admin@gmail.com" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin-seed-id",
                columns: new[] { "Email", "NormalizedEmail", "NormalizedUserName", "UserName" },
                values: new object[] { "admin@eshoppingzone.com", "ADMIN@ESHOPPINGZONE.COM", "ADMIN@ESHOPPINGZONE.COM", "admin@eshoppingzone.com" });
        }
    }
}
