using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazingPizza.Migrations
{
    /// <inheritdoc />
    public partial class editProjectLogicMIG : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Address_DeliveryAddressId" ,
                table: "Orders");

            migrationBuilder.DropTable(
                name: "Address");

            migrationBuilder.DropTable(
                name: "PizzaTopping");

            migrationBuilder.DropTable(
                name: "Pizzas");

            migrationBuilder.DropTable(
                name: "Toppings");

            migrationBuilder.DropTable(
                name: "Specials");

            migrationBuilder.DropIndex(
                name: "IX_Orders_DeliveryAddressId" ,
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "DeliveryLocation_Latitude" ,
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "DeliveryLocation_Longitude" ,
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "DeliveryAddressId" ,
                table: "Orders" ,
                newName: "CarrierId");

            migrationBuilder.AddColumn<string>(
                name: "CarrierName" ,
                table: "Orders" ,
                type: "nvarchar(max)" ,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Carriers" ,
                columns: table => new
                {
                    id = table.Column<int>(type: "int" , nullable: false)
                        .Annotation("SqlServer:Identity" , "1, 1") ,
                    carrierName = table.Column<string>(type: "nvarchar(max)" , nullable: false) ,
                    isActive = table.Column<bool>(type: "bit" , nullable: false) ,
                    UnderMaintenance = table.Column<bool>(type: "bit" , nullable: true) ,
                    wholeSalePercent = table.Column<double>(type: "float" , nullable: false) ,
                    internetSubscribEmail = table.Column<string>(type: "nvarchar(max)" , nullable: false) ,
                    ImeiUserName = table.Column<string>(type: "nvarchar(max)" , nullable: false) ,
                    ImeiAPIKey = table.Column<string>(type: "nvarchar(max)" , nullable: false) ,
                    ImeiAPIURL = table.Column<string>(type: "nvarchar(max)" , nullable: false) ,
                    APIPassword = table.Column<string>(type: "nvarchar(max)" , nullable: false) ,
                    AdsSendSMS = table.Column<bool>(type: "bit" , nullable: false) ,
                    AdsSendWP = table.Column<bool>(type: "bit" , nullable: false) ,
                    InvoiceCategory = table.Column<int>(type: "int" , nullable: true)
                } ,
                constraints: table => {
                    table.PrimaryKey("PK_Carriers" , x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "RefillCardOrder" ,
                columns: table => new
                {
                    RefillCardId = table.Column<int>(type: "int" , nullable: false) ,
                    ProductName = table.Column<string>(type: "nvarchar(max)" , nullable: false) ,
                    Quantity = table.Column<int>(type: "int" , nullable: false) ,
                    UnitPrice = table.Column<double>(type: "float" , nullable: false)
                } ,
                constraints: table => {
                });

            migrationBuilder.CreateTable(
                name: "RefillCards" ,
                columns: table => new
                {
                    id = table.Column<int>(type: "int" , nullable: false)
                        .Annotation("SqlServer:Identity" , "1, 1") ,
                    ProductName = table.Column<string>(type: "nvarchar(max)" , nullable: false) ,
                    CardAmount = table.Column<int>(type: "int" , nullable: false) ,
                    CarrierID = table.Column<int>(type: "int" , nullable: false) ,
                    apiProductId = table.Column<int>(type: "int" , nullable: true) ,
                    description = table.Column<string>(type: "nvarchar(max)" , nullable: false) ,
                    isActive = table.Column<bool>(type: "bit" , nullable: false) ,
                    isTemp = table.Column<bool>(type: "bit" , nullable: false) ,
                    imageName = table.Column<string>(type: "nvarchar(max)" , nullable: false) ,
                    ProductIdenfity = table.Column<string>(type: "nvarchar(max)" , nullable: false) ,
                    CreatedOn = table.Column<DateTime>(type: "datetime2" , nullable: false) ,
                    UpdatedOn = table.Column<DateTime>(type: "datetime2" , nullable: true) ,
                    price = table.Column<double>(type: "float" , nullable: false) ,
                    wholeSalePercent = table.Column<double>(type: "float" , nullable: false) ,
                    Cost = table.Column<double>(type: "float" , nullable: false) ,
                    ProductNameHe = table.Column<string>(type: "nvarchar(max)" , nullable: false) ,
                    ProductNameEn = table.Column<string>(type: "nvarchar(max)" , nullable: false) ,
                    DescriptionHe = table.Column<string>(type: "nvarchar(max)" , nullable: false) ,
                    DescriptionEn = table.Column<string>(type: "nvarchar(max)" , nullable: false) ,
                    SendByEmail = table.Column<bool>(type: "bit" , nullable: false) ,
                    EmailCode = table.Column<string>(type: "nvarchar(max)" , nullable: false) ,
                    ApiProductId1 = table.Column<int>(type: "int" , nullable: true) ,
                    ApiProductId2 = table.Column<int>(type: "int" , nullable: true) ,
                    ApiProductIdStr = table.Column<string>(type: "nvarchar(max)" , nullable: false) ,
                    SiteId = table.Column<int>(type: "int" , nullable: true) ,
                    SortOrder = table.Column<int>(type: "int" , nullable: false) ,
                    IsFav = table.Column<bool>(type: "bit" , nullable: false) ,
                    Group = table.Column<int>(type: "int" , nullable: true) ,
                    RemoteProviderId = table.Column<int>(type: "int" , nullable: true) ,
                    DetailsUrl = table.Column<string>(type: "nvarchar(max)" , nullable: false) ,
                    Lotto = table.Column<bool>(type: "bit" , nullable: false) ,
                    PriceExcempt = table.Column<decimal>(type: "decimal(18,2)" , nullable: false) ,
                    PointsEndUser = table.Column<int>(type: "int" , nullable: false) ,
                    PointsUser = table.Column<int>(type: "int" , nullable: false) ,
                    PointsReseller = table.Column<int>(type: "int" , nullable: false) ,
                    SendMsgBy = table.Column<string>(type: "nvarchar(max)" , nullable: false) ,
                    AdsCardSms = table.Column<string>(type: "nvarchar(max)" , nullable: false) ,
                    AdsCardWhatsapp = table.Column<string>(type: "nvarchar(max)" , nullable: false)
                } ,
                constraints: table => {
                    table.PrimaryKey("PK_RefillCards" , x => x.id);
                    table.ForeignKey(
                        name: "FK_RefillCards_Carriers_CarrierID" ,
                        column: x => x.CarrierID ,
                        principalTable: "Carriers" ,
                        principalColumn: "id" ,
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RefillCards_CarrierID" ,
                table: "RefillCards" ,
                column: "CarrierID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RefillCardOrder");

            migrationBuilder.DropTable(
                name: "RefillCards");

            migrationBuilder.DropTable(
                name: "Carriers");

            migrationBuilder.DropColumn(
                name: "CarrierName" ,
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "CarrierId" ,
                table: "Orders" ,
                newName: "DeliveryAddressId");

            migrationBuilder.AddColumn<double>(
                name: "DeliveryLocation_Latitude" ,
                table: "Orders" ,
                type: "float" ,
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "DeliveryLocation_Longitude" ,
                table: "Orders" ,
                type: "float" ,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Address" ,
                columns: table => new
                {
                    Id = table.Column<int>(type: "int" , nullable: false)
                        .Annotation("SqlServer:Identity" , "1, 1") ,
                    City = table.Column<string>(type: "nvarchar(50)" , maxLength: 50 , nullable: false) ,
                    Line1 = table.Column<string>(type: "nvarchar(100)" , maxLength: 100 , nullable: false) ,
                    Line2 = table.Column<string>(type: "nvarchar(100)" , maxLength: 100 , nullable: false) ,
                    Name = table.Column<string>(type: "nvarchar(100)" , maxLength: 100 , nullable: false) ,
                    PostalCode = table.Column<string>(type: "nvarchar(20)" , maxLength: 20 , nullable: false) ,
                    Region = table.Column<string>(type: "nvarchar(20)" , maxLength: 20 , nullable: false)
                } ,
                constraints: table => {
                    table.PrimaryKey("PK_Address" , x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Specials" ,
                columns: table => new
                {
                    Id = table.Column<int>(type: "int" , nullable: false)
                        .Annotation("SqlServer:Identity" , "1, 1") ,
                    BasePrice = table.Column<decimal>(type: "decimal(18,2)" , nullable: false) ,
                    Description = table.Column<string>(type: "nvarchar(max)" , nullable: false) ,
                    ImageUrl = table.Column<string>(type: "nvarchar(max)" , nullable: false) ,
                    Name = table.Column<string>(type: "nvarchar(max)" , nullable: false)
                } ,
                constraints: table => {
                    table.PrimaryKey("PK_Specials" , x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Toppings" ,
                columns: table => new
                {
                    Id = table.Column<int>(type: "int" , nullable: false)
                        .Annotation("SqlServer:Identity" , "1, 1") ,
                    Name = table.Column<string>(type: "nvarchar(max)" , nullable: false) ,
                    Price = table.Column<decimal>(type: "decimal(18,2)" , nullable: false)
                } ,
                constraints: table => {
                    table.PrimaryKey("PK_Toppings" , x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Pizzas" ,
                columns: table => new
                {
                    Id = table.Column<int>(type: "int" , nullable: false)
                        .Annotation("SqlServer:Identity" , "1, 1") ,
                    SpecialId = table.Column<int>(type: "int" , nullable: false) ,
                    OrderId = table.Column<int>(type: "int" , nullable: false) ,
                    Size = table.Column<int>(type: "int" , nullable: false)
                } ,
                constraints: table => {
                    table.PrimaryKey("PK_Pizzas" , x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pizzas_Orders_OrderId" ,
                        column: x => x.OrderId ,
                        principalTable: "Orders" ,
                        principalColumn: "OrderId" ,
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Pizzas_Specials_SpecialId" ,
                        column: x => x.SpecialId ,
                        principalTable: "Specials" ,
                        principalColumn: "Id" ,
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PizzaTopping" ,
                columns: table => new
                {
                    PizzaId = table.Column<int>(type: "int" , nullable: false) ,
                    ToppingId = table.Column<int>(type: "int" , nullable: false)
                } ,
                constraints: table => {
                    table.PrimaryKey("PK_PizzaTopping" , x => new { x.PizzaId , x.ToppingId });
                    table.ForeignKey(
                        name: "FK_PizzaTopping_Pizzas_PizzaId" ,
                        column: x => x.PizzaId ,
                        principalTable: "Pizzas" ,
                        principalColumn: "Id" ,
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PizzaTopping_Toppings_ToppingId" ,
                        column: x => x.ToppingId ,
                        principalTable: "Toppings" ,
                        principalColumn: "Id" ,
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_DeliveryAddressId" ,
                table: "Orders" ,
                column: "DeliveryAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Pizzas_OrderId" ,
                table: "Pizzas" ,
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Pizzas_SpecialId" ,
                table: "Pizzas" ,
                column: "SpecialId");

            migrationBuilder.CreateIndex(
                name: "IX_PizzaTopping_ToppingId" ,
                table: "PizzaTopping" ,
                column: "ToppingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Address_DeliveryAddressId" ,
                table: "Orders" ,
                column: "DeliveryAddressId" ,
                principalTable: "Address" ,
                principalColumn: "Id" ,
                onDelete: ReferentialAction.Cascade);
        }
    }
}
