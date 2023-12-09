using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rocky_DataAccess.Migrations
{
    public partial class ProductCh2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_Warehouse_LoadingWarehouseId",
                table: "Product");

            migrationBuilder.DropForeignKey(
                name: "FK_Product_Warehouse_UnloadingWarehouseId",
                table: "Product");

            migrationBuilder.AlterColumn<int>(
                name: "UnloadingWarehouseId",
                table: "Product",
                type: "int",
                nullable: false,
                defaultValue: 1,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "LoadingWarehouseId",
                table: "Product",
                type: "int",
                nullable: false,
                defaultValue: 1,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Warehouse_LoadingWarehouseId",
                table: "Product",
                column: "LoadingWarehouseId",
                principalTable: "Warehouse",
                principalColumn: "WarehouseId",
                onDelete: ReferentialAction.SetDefault);

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Warehouse_UnloadingWarehouseId",
                table: "Product",
                column: "UnloadingWarehouseId",
                principalTable: "Warehouse",
                principalColumn: "WarehouseId",
                onDelete: ReferentialAction.SetDefault);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_Warehouse_LoadingWarehouseId",
                table: "Product");

            migrationBuilder.DropForeignKey(
                name: "FK_Product_Warehouse_UnloadingWarehouseId",
                table: "Product");

            migrationBuilder.AlterColumn<int>(
                name: "UnloadingWarehouseId",
                table: "Product",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "LoadingWarehouseId",
                table: "Product",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Warehouse_LoadingWarehouseId",
                table: "Product",
                column: "LoadingWarehouseId",
                principalTable: "Warehouse",
                principalColumn: "WarehouseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Warehouse_UnloadingWarehouseId",
                table: "Product",
                column: "UnloadingWarehouseId",
                principalTable: "Warehouse",
                principalColumn: "WarehouseId");
        }
    }
}
