using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HTML__Parser.Migrations
{
    /// <inheritdoc />
    public partial class Update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Value",
                table: "Products",
                newName: "Vendor");

            migrationBuilder.RenameColumn(
                name: "ProductName",
                table: "Products",
                newName: "Code");

            migrationBuilder.RenameColumn(
                name: "LastUpdate",
                table: "Products",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "FirstCreate",
                table: "Products",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "Discription",
                table: "Products",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "Avaliable",
                table: "Products",
                newName: "Available");

            migrationBuilder.RenameIndex(
                name: "IX_Products_ProductName",
                table: "Products",
                newName: "IX_Products_Code");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Vendor",
                table: "Products",
                newName: "Value");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "Products",
                newName: "LastUpdate");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Products",
                newName: "Discription");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Products",
                newName: "FirstCreate");

            migrationBuilder.RenameColumn(
                name: "Code",
                table: "Products",
                newName: "ProductName");

            migrationBuilder.RenameColumn(
                name: "Available",
                table: "Products",
                newName: "Avaliable");

            migrationBuilder.RenameIndex(
                name: "IX_Products_Code",
                table: "Products",
                newName: "IX_Products_ProductName");
        }
    }
}
