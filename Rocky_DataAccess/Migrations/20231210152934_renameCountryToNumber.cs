using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rocky_DataAccess.Migrations
{
    public partial class renameCountryToNumber : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Country",
                table: "Warehouse",
                newName: "Number");

            migrationBuilder.RenameColumn(
                name: "WarehouseId",
                table: "Warehouse",
                newName: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Number",
                table: "Warehouse",
                newName: "Country");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Warehouse",
                newName: "WarehouseId");
        }
    }
}
