using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HD.Wallet.Account.Service.Migrations
{
    /// <inheritdoc />
    public partial class AddBLockOrUnlinkedCol : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsBlocked",
                table: "Account",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsUnlinked",
                table: "Account",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsBlocked",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "IsUnlinked",
                table: "Account");
        }
    }
}
