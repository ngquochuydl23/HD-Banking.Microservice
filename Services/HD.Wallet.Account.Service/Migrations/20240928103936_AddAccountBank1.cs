using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HD.Wallet.Account.Service.Migrations
{
    /// <inheritdoc />
    public partial class AddAccountBank1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AccountBank_BankAccountId",
                table: "Account",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AccountBank_BankName",
                table: "Account",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AccountBank_BankOwnerName",
                table: "Account",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AccountBank_IdCardNo",
                table: "Account",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountBank_BankAccountId",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "AccountBank_BankName",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "AccountBank_BankOwnerName",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "AccountBank_IdCardNo",
                table: "Account");
        }
    }
}
