using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SMB.INFRASTRUCTURE.Migrations
{
    /// <inheritdoc />
    public partial class StatusIdReferenceCatalog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "StatusId",
                table: "catalog_language",
                type: "bigint",
                nullable: false,
                defaultValue: 2L);

            migrationBuilder.AddColumn<long>(
                name: "StatusId",
                table: "catalog_currency",
                type: "bigint",
                nullable: false,
                defaultValue: 2L);

            migrationBuilder.CreateIndex(
                name: "IX_catalog_language_StatusId",
                table: "catalog_language",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_catalog_currency_StatusId",
                table: "catalog_currency",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_catalog_currency_catalog_status_StatusId",
                table: "catalog_currency",
                column: "StatusId",
                principalTable: "catalog_status",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_catalog_language_catalog_status_StatusId",
                table: "catalog_language",
                column: "StatusId",
                principalTable: "catalog_status",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_catalog_currency_catalog_status_StatusId",
                table: "catalog_currency");

            migrationBuilder.DropForeignKey(
                name: "FK_catalog_language_catalog_status_StatusId",
                table: "catalog_language");

            migrationBuilder.DropIndex(
                name: "IX_catalog_language_StatusId",
                table: "catalog_language");

            migrationBuilder.DropIndex(
                name: "IX_catalog_currency_StatusId",
                table: "catalog_currency");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "catalog_language");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "catalog_currency");
        }
    }
}
