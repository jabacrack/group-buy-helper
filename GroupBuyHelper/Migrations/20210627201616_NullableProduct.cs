using Microsoft.EntityFrameworkCore.Migrations;

namespace GroupBuyHelper.Migrations
{
    public partial class NullableProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_ProductLists_ProductsListId",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "ProductsListId",
                table: "Products",
                newName: "ProductListId");

            migrationBuilder.RenameIndex(
                name: "IX_Products_ProductsListId",
                table: "Products",
                newName: "IX_Products_ProductListId");

            migrationBuilder.AlterColumn<double>(
                name: "Price",
                table: "Products",
                type: "REAL",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "REAL");

            migrationBuilder.AlterColumn<int>(
                name: "Amount",
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_ProductLists_ProductListId",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "ProductListId",
                table: "Products",
                newName: "ProductsListId");

            migrationBuilder.RenameIndex(
                name: "IX_Products_ProductListId",
                table: "Products",
                newName: "IX_Products_ProductsListId");

            migrationBuilder.AlterColumn<double>(
                name: "Price",
                table: "Products",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "REAL",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Amount",
                table: "Products",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ProductLists_ProductsListId",
                table: "Products",
                column: "ProductsListId",
                principalTable: "ProductLists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
