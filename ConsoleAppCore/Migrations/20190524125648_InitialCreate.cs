using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ConsoleAppCore.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserProfile",
                columns: table => new
                {
                    UserID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserName = table.Column<string>(maxLength: 50, nullable: false),
                    FirstName = table.Column<string>(maxLength: 50, nullable: false),
                    LastName = table.Column<string>(maxLength: 50, nullable: false),
                    Password = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfile", x => x.UserID);
                });

            migrationBuilder.CreateTable(
                name: "ShoppingList",
                columns: table => new
                {
                    ShoppingListID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(maxLength: 50, nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    UserProfileUserID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingList", x => x.ShoppingListID);
                    table.ForeignKey(
                        name: "FK_ShoppingList_UserProfile_UserProfileUserID",
                        column: x => x.UserProfileUserID,
                        principalTable: "UserProfile",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ShoppingListItem",
                columns: table => new
                {
                    ItemID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    ShoppingListID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingListItem", x => x.ItemID);
                    table.ForeignKey(
                        name: "FK_ShoppingListItem_ShoppingList_ShoppingListID",
                        column: x => x.ShoppingListID,
                        principalTable: "ShoppingList",
                        principalColumn: "ShoppingListID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingList_UserProfileUserID",
                table: "ShoppingList",
                column: "UserProfileUserID");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingListItem_ShoppingListID",
                table: "ShoppingListItem",
                column: "ShoppingListID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShoppingListItem");

            migrationBuilder.DropTable(
                name: "ShoppingList");

            migrationBuilder.DropTable(
                name: "UserProfile");
        }
    }
}
