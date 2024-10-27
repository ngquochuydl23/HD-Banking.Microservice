using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HD.Wallet.Transaction.Service.Migrations
{
    /// <inheritdoc />
    public partial class FixAccountBankValueObject13 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UseSourceLinkingBank",
                table: "Transaction",
                newName: "UseSourceAsLinkingBank");

            migrationBuilder.RenameColumn(
                name: "IsTranferedBank",
                table: "Transaction",
                newName: "IsBankingTransfer");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UseSourceAsLinkingBank",
                table: "Transaction",
                newName: "UseSourceLinkingBank");

            migrationBuilder.RenameColumn(
                name: "IsBankingTransfer",
                table: "Transaction",
                newName: "IsTranferedBank");
        }
    }
}
