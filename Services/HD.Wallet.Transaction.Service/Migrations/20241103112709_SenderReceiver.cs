using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HD.Wallet.Transaction.Service.Migrations
{
    /// <inheritdoc />
    public partial class SenderReceiver : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ReceiverUserId",
                table: "Transaction",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SenderUserId",
                table: "Transaction",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReceiverUserId",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "SenderUserId",
                table: "Transaction");
        }
    }
}
