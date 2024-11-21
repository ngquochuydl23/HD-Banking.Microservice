using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HD.Wallet.Account.Service.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSaved : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountNo",
                table: "SavedDestination");

            migrationBuilder.DropColumn(
                name: "BankFullName",
                table: "SavedDestination");

            migrationBuilder.DropColumn(
                name: "BankLogo",
                table: "SavedDestination");

            migrationBuilder.DropColumn(
                name: "BankShortName",
                table: "SavedDestination");

            migrationBuilder.DropColumn(
                name: "Bin",
                table: "SavedDestination");

            migrationBuilder.DropColumn(
                name: "OwnerName",
                table: "SavedDestination");

            migrationBuilder.DropColumn(
                name: "SavedName",
                table: "SavedDestination");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AccountNo",
                table: "SavedDestination",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BankFullName",
                table: "SavedDestination",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BankLogo",
                table: "SavedDestination",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BankShortName",
                table: "SavedDestination",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Bin",
                table: "SavedDestination",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OwnerName",
                table: "SavedDestination",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SavedName",
                table: "SavedDestination",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
