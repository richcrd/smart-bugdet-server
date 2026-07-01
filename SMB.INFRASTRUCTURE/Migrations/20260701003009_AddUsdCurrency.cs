using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SMB.INFRASTRUCTURE.Migrations
{
    /// <inheritdoc />
    public partial class AddUsdCurrency : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "catalog_currency",
                columns: new[] { "Code", "Name", "Symbol", "DecimalPlaces", "StatusId", "CreatedAt" },
                values: new object[] { "USD", "Dólar estadounidense", "$", 2, 2, DateTime.UtcNow });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "catalog_currency",
                keyColumn: "Code",
                keyValue: "USD");
        }
    }
}
