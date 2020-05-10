using Microsoft.EntityFrameworkCore.Migrations;

namespace SocialNetwork.Migrations
{
    public partial class AddCountryCities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CountryID",
                table: "User",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_User_CountryID",
                table: "User",
                column: "CountryID");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Country_CountryID",
                table: "User",
                column: "CountryID",
                principalTable: "Country",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropForeignKey(
                name: "FK_User_Country_CountryID",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_CountryID",
                table: "User");

            migrationBuilder.DropColumn(
                name: "CountryID",
                table: "User");
        }
    }
}
