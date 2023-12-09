using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rocky_DataAccess.Migrations
{
    public partial class ProductCh : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LoadingWarehouseId",
                table: "Product",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UnloadingWarehouseId",
                table: "Product",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Warehouse",
                columns: table => new
                {
                    WarehouseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Street = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ZipCode = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Warehouse", x => x.WarehouseId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Product_LoadingWarehouseId",
                table: "Product",
                column: "LoadingWarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_UnloadingWarehouseId",
                table: "Product",
                column: "UnloadingWarehouseId");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_Warehouse_LoadingWarehouseId",
                table: "Product");

            migrationBuilder.DropForeignKey(
                name: "FK_Product_Warehouse_UnloadingWarehouseId",
                table: "Product");

            migrationBuilder.DropTable(
                name: "Warehouse");

            migrationBuilder.DropIndex(
                name: "IX_Product_LoadingWarehouseId",
                table: "Product");

            migrationBuilder.DropIndex(
                name: "IX_Product_UnloadingWarehouseId",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "LoadingWarehouseId",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "UnloadingWarehouseId",
                table: "Product");
        }
    }
}
