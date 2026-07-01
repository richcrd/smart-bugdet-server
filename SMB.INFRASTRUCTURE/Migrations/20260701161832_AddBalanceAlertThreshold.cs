using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SMB.INFRASTRUCTURE.Migrations
{
    /// <inheritdoc />
    public partial class AddBalanceAlertThreshold : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "BalanceAlertThreshold",
                table: "user_preferences",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BalanceAlertThreshold",
                table: "user_preferences");
        }
    }
}
