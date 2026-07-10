using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eshoppingzone.Migrations
{
    /// <inheritdoc />
    public partial class SeedAdminUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "IsApproved", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "Role", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "admin-seed-id", 0, "ADMIN-CONCURRENCY-STAMP", "admin@eshoppingzone.com", true, true, false, null, "ADMIN@ESHOPPINGZONE.COM", "ADMIN@ESHOPPINGZONE.COM", "AQAAAAIAAYagAAAAEJ8fHQqLxvbqJZ8fHQqLxvbqJZ8fHQqLxvbqJZ8fHQqLxvbqJZ8fHQqLxvbqJQ==", null, false, "Admin", "ADMIN-SECURITY-STAMP", false, "admin@eshoppingzone.com" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin-seed-id");
        }
    }
}
