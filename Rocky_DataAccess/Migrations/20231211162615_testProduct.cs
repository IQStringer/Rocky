using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rocky_DataAccess.Migrations
{
    public partial class testProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DeliveryTime",
                table: "Product",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeliveryTime",
                table: "Product");
        }
    }
}
