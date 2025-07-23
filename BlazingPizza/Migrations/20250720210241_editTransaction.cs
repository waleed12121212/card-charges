using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazingPizza.Migrations
{
    /// <inheritdoc />
    public partial class editTransaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RefillCardOrderId",
                table: "Transactions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_RefillCardOrderId",
                table: "Transactions",
                column: "RefillCardOrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_RefillCardOrders_RefillCardOrderId",
                table: "Transactions",
                column: "RefillCardOrderId",
                principalTable: "RefillCardOrders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_RefillCardOrders_RefillCardOrderId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_RefillCardOrderId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "RefillCardOrderId",
                table: "Transactions");
        }
    }
}
