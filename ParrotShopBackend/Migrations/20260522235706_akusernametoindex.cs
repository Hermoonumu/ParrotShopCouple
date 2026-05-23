using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParrotShopBackend.Migrations
{
    /// <inheritdoc />
    public partial class akusernametoindex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_Users_Username",
                table: "Users");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_Username",
                table: "Users");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Users_Username",
                table: "Users",
                column: "Username");
        }
    }
}
