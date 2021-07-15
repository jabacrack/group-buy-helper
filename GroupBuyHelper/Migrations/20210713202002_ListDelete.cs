using Microsoft.EntityFrameworkCore.Migrations;

namespace GroupBuyHelper.Migrations
{
    public partial class ListDelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_ProductLists_ProductListId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_UserOrderItems_AspNetUsers_OwnerId",
                table: "UserOrderItems");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ProductLists_ProductListId",
                table: "Products",
                column: "ProductListId",
                principalTable: "ProductLists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserOrderItems_AspNetUsers_OwnerId",
                table: "UserOrderItems",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_ProductLists_ProductListId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_UserOrderItems_AspNetUsers_OwnerId",
                table: "UserOrderItems");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ProductLists_ProductListId",
                table: "Products",
                column: "ProductListId",
                principalTable: "ProductLists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserOrderItems_AspNetUsers_OwnerId",
                table: "UserOrderItems",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
