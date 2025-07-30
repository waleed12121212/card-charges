using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazingPizza.Migrations
{
    /// <inheritdoc />
    public partial class AddInternetPackagesWithCarrierType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InternetPackages_Carriers_CarrierId",
                table: "InternetPackages");

            migrationBuilder.DropIndex(
                name: "IX_InternetPackages_CarrierId",
                table: "InternetPackages");

            migrationBuilder.RenameColumn(
                name: "CarrierId",
                table: "InternetPackages",
                newName: "CarrierType");

            migrationBuilder.AddColumn<int>(
                name: "CarrierType",
                table: "InternetPackagePurchases",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CarrierType",
                table: "InternetPackagePurchases");

            migrationBuilder.RenameColumn(
                name: "CarrierType",
                table: "InternetPackages",
                newName: "CarrierId");

            migrationBuilder.CreateIndex(
                name: "IX_InternetPackages_CarrierId",
                table: "InternetPackages",
                column: "CarrierId");

            migrationBuilder.AddForeignKey(
                name: "FK_InternetPackages_Carriers_CarrierId",
                table: "InternetPackages",
                column: "CarrierId",
                principalTable: "Carriers",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
