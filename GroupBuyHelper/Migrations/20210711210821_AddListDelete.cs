using Microsoft.EntityFrameworkCore.Migrations;

namespace GroupBuyHelper.Migrations
{
    public partial class AddListDelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_ProductLists_ProductListId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_UserOrderItems_AspNetUsers_OwnerId",
                table: "UserOrderItems");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "UserOrderItems",
                type: "TEXT",
                nullable: false);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ProductLists_ProductListId",
                table: "Products",
                column: "ProductListId",
                principalTable: "ProductLists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ProductLists_ProductListId1",
                table: "Products",
                column: "ProductListId1",
                principalTable: "ProductLists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserOrderItems_AspNetUsers_ApplicationUserId",
                table: "UserOrderItems",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserOrderItems_AspNetUsers_OwnerId",
                table: "UserOrderItems",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserOrderItems_ProductLists_ProductListId1",
                table: "UserOrderItems",
                column: "ProductListId1",
                principalTable: "ProductLists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserOrderItems_Products_ProductId1",
                table: "UserOrderItems",
                column: "ProductId1",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_ProductLists_ProductListId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_ProductLists_ProductListId1",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_UserOrderItems_AspNetUsers_ApplicationUserId",
                table: "UserOrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_UserOrderItems_AspNetUsers_OwnerId",
                table: "UserOrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_UserOrderItems_ProductLists_ProductListId1",
                table: "UserOrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_UserOrderItems_Products_ProductId1",
                table: "UserOrderItems");

            migrationBuilder.DropIndex(
                name: "IX_UserOrderItems_ApplicationUserId",
                table: "UserOrderItems");

            migrationBuilder.DropIndex(
                name: "IX_UserOrderItems_ProductId1",
                table: "UserOrderItems");

            migrationBuilder.DropIndex(
                name: "IX_UserOrderItems_ProductListId1",
                table: "UserOrderItems");

            migrationBuilder.DropIndex(
                name: "IX_Products_ProductListId1",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "UserOrderItems");

            migrationBuilder.DropColumn(
                name: "ProductId1",
                table: "UserOrderItems");

            migrationBuilder.DropColumn(
                name: "ProductListId1",
                table: "UserOrderItems");

            migrationBuilder.DropColumn(
                name: "ProductListId1",
                table: "Products");

            migrationBuilder.AlterColumn<int>(
                name: "ProductListId",
                table: "Products",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

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
