using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HD.Wallet.Transaction.Service.Migrations
{
    /// <inheritdoc />
    public partial class AddBankAccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AfterBalance",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "BeforeBalance",
                table: "Transaction");

            migrationBuilder.RenameColumn(
                name: "SenderAccountId",
                table: "Transaction",
                newName: "SourceAccount_OwnerName");

            migrationBuilder.RenameColumn(
                name: "ReceiverAccountId",
                table: "Transaction",
                newName: "SourceAccount_Bin");

            migrationBuilder.AddColumn<string>(
                name: "DestAccount_AccountNo",
                table: "Transaction",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DestAccount_Bin",
                table: "Transaction",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DestAccount_OwnerName",
                table: "Transaction",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SourceAccount_AccountNo",
                table: "Transaction",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DestAccount_AccountNo",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "DestAccount_Bin",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "DestAccount_OwnerName",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "SourceAccount_AccountNo",
                table: "Transaction");

            migrationBuilder.RenameColumn(
                name: "SourceAccount_OwnerName",
                table: "Transaction",
                newName: "SenderAccountId");

            migrationBuilder.RenameColumn(
                name: "SourceAccount_Bin",
                table: "Transaction",
                newName: "ReceiverAccountId");

            migrationBuilder.AddColumn<double>(
                name: "AfterBalance",
                table: "Transaction",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BeforeBalance",
                table: "Transaction",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
