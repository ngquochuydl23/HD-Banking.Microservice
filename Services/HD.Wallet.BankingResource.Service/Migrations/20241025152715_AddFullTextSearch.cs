using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HD.Wallet.BankingResource.Service.Migrations
{
    /// <inheritdoc />
    public partial class AddFullTextSearch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            
            migrationBuilder.CreateIndex(
                name: "IX_Banks_Name_ShortName_Bin",
                table: "Banks",
                columns: new[] { "Name", "ShortName", "Bin" });

    
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
           
        }
    }
}
