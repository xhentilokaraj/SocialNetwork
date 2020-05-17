using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SocialNetwork.Migrations
{
    public partial class UserRelationships : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserRelationship",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RelatingUserId = table.Column<int>(nullable: false),
                    RelatedUserId = table.Column<int>(nullable: false),
                    RequestStatus = table.Column<string>(nullable: true),
                    RelationshipStatus = table.Column<string>(nullable: true),
                    DateOfRequest = table.Column<DateTime>(nullable: false, defaultValueSql: "getutcdate()"),
                    DateOfAcceptance = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRelationship", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRelationship_User_RelatedUserId",
                        column: x => x.RelatedUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_UserRelationship_User_RelatingUserId",
                        column: x => x.RelatingUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserRelationship_RelatedUserId",
                table: "UserRelationship",
                column: "RelatedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRelationship_RelatingUserId",
                table: "UserRelationship",
                column: "RelatingUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserRelationship");
        }
    }
}
