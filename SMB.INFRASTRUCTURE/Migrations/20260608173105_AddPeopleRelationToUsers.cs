using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SMB.INFRASTRUCTURE.Migrations
{
    /// <inheritdoc />
    public partial class AddPeopleRelationToUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "PeopleId",
                table: "users",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_users_PeopleId",
                table: "users",
                column: "PeopleId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_users_people_PeopleId",
                table: "users",
                column: "PeopleId",
                principalTable: "people",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_users_people_PeopleId",
                table: "users");

            migrationBuilder.DropIndex(
                name: "IX_users_PeopleId",
                table: "users");

            migrationBuilder.DropColumn(
                name: "PeopleId",
                table: "users");
        }
    }
}
