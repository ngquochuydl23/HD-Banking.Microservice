using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HD.Wallet.Transaction.Service.Migrations
{
    /// <inheritdoc />
    public partial class FixAccountBankValueObject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DestAccount_BankFullName",
                table: "Transaction",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DestAccount_LogoUrl",
                table: "Transaction",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DestAccount_ShortName",
                table: "Transaction",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SourceAccount_BankFullName",
                table: "Transaction",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SourceAccount_LogoUrl",
                table: "Transaction",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SourceAccount_ShortName",
                table: "Transaction",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DestAccount_BankFullName",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "DestAccount_LogoUrl",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "DestAccount_ShortName",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "SourceAccount_BankFullName",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "SourceAccount_LogoUrl",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "SourceAccount_ShortName",
                table: "Transaction");
        }
    }
}
