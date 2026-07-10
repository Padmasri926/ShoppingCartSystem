using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eshoppingzone.Migrations
{
    /// <inheritdoc />
    public partial class RemoveHardcodedAdminSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin-seed-id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "IsApproved", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "Role", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "admin-seed-id", 0, "ADMIN-CONCURRENCY-STAMP", "admin@gmail.com", true, true, false, null, "ADMIN@GMAIL.COM", "ADMIN@GMAIL.COM", "AQAAAAEAACcQAAAAEI9y1k6DnupfaNmgPsFGu1I1OTCxJdPZj4hssewGeJygWn3pHyCcyXAbQtOcPHLCdw==", null, false, "Admin", "ADMIN-SECURITY-STAMP", false, "admin@gmail.com" });
        }
    }
}
