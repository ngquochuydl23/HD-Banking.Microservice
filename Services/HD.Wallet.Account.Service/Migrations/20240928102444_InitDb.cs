using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HD.Wallet.Account.Service.Migrations
{
    /// <inheritdoc />
    public partial class InitDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    PhoneNumber = table.Column<string>(type: "text", nullable: false),
                    FullName = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Sex = table.Column<int>(type: "integer", nullable: false),
                    IdCardNo = table.Column<string>(type: "text", nullable: false),
                    Nationality = table.Column<string>(type: "text", nullable: false),
                    PlaceOfOrigin = table.Column<string>(type: "text", nullable: false),
                    PlaceOfResidence = table.Column<string>(type: "text", nullable: false),
                    DateOfExpiry = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    FrontIdCardUrl = table.Column<string>(type: "text", nullable: false),
                    BackIdCardUrl = table.Column<string>(type: "text", nullable: false),
                    IdCardType = table.Column<string>(type: "text", nullable: false),
                    IsEkycVerfied = table.Column<bool>(type: "boolean", nullable: false),
                    FaceVerificationUrl = table.Column<string>(type: "text", nullable: false),
                    Avatar = table.Column<string>(type: "text", nullable: false),
                    HashPassword = table.Column<string>(type: "text", nullable: false),
                    PinPassword = table.Column<string>(type: "text", nullable: false),
                    AccountStatus = table.Column<int>(type: "integer", nullable: false),
                    Address_Street = table.Column<string>(type: "text", nullable: false),
                    Address_ProvinceOrCity = table.Column<string>(type: "text", nullable: false),
                    Address_District = table.Column<string>(type: "text", nullable: false),
                    Address_WardOrCommune = table.Column<string>(type: "text", nullable: false),
                    Work_Occupation = table.Column<string>(type: "text", nullable: false),
                    Work_Position = table.Column<string>(type: "text", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Account",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    IsBankLinking = table.Column<bool>(type: "boolean", nullable: false),
                    WalletBalance = table.Column<double>(type: "double precision", nullable: false),
                    LinkedAccountId = table.Column<string>(type: "text", nullable: false),
                    TransactionLimit = table.Column<int>(type: "integer", nullable: false),
                    AccountType = table.Column<int>(type: "integer", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Account_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Account_UserId",
                table: "Account",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Account");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
