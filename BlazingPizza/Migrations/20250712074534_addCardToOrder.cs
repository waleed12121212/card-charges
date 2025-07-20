using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazingPizza.Migrations
{
    /// <inheritdoc />
    public partial class addCardToOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "RefillCardOrder",
                newName: "RefillCardOrders");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "RefillCardOrders",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "RefillCardOrders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_RefillCardOrders",
                table: "RefillCardOrders",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_RefillCardOrders_OrderId",
                table: "RefillCardOrders",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_RefillCardOrders_Orders_OrderId",
                table: "RefillCardOrders",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "OrderId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RefillCardOrders_Orders_OrderId",
                table: "RefillCardOrders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RefillCardOrders",
                table: "RefillCardOrders");

            migrationBuilder.DropIndex(
                name: "IX_RefillCardOrders_OrderId",
                table: "RefillCardOrders");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "RefillCardOrders");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "RefillCardOrders");

            migrationBuilder.RenameTable(
                name: "RefillCardOrders",
                newName: "RefillCardOrder");
        }
    }
}
