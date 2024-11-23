using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HD.Wallet.Account.Service.Migrations
{
    /// <inheritdoc />
    public partial class CurrentLiving : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CurrentLiving",
                table: "User",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MarrigeStatus",
                table: "User",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "StudyLevel",
                table: "User",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentLiving",
                table: "User");

            migrationBuilder.DropColumn(
                name: "MarrigeStatus",
                table: "User");

            migrationBuilder.DropColumn(
                name: "StudyLevel",
                table: "User");
        }
    }
}
