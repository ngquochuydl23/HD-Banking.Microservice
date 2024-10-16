using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace HD.Wallet.Account.Service.Migrations
{
    /// <inheritdoc />
    public partial class LogoBank : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AccountBank_BankFullName",
                table: "Account",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AccountBank_Bin",
                table: "Account",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AccountBank_LogoUrl",
                table: "Account",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Contact",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OwnerId = table.Column<string>(type: "text", nullable: false),
                    ReferenceUserId = table.Column<string>(type: "text", nullable: false),
                    ContactType = table.Column<string>(type: "text", nullable: false),
                    SavedName = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contact", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Contact_User_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Contact_User_ReferenceUserId",
                        column: x => x.ReferenceUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Contact_OwnerId",
                table: "Contact",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Contact_ReferenceUserId",
                table: "Contact",
                column: "ReferenceUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Contact");

            migrationBuilder.DropColumn(
                name: "AccountBank_BankFullName",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "AccountBank_Bin",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "AccountBank_LogoUrl",
                table: "Account");
        }
    }
}
