using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShoeTrackr.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "items",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    price = table.Column<int>(type: "int", nullable: false),
                    quantity = table.Column<int>(type: "int", nullable: false),
                    rating = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_items", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "sales",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    company = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    netPrice = table.Column<int>(type: "int", nullable: false),
                    netQuantity = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sales", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "purchases",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    item_id = table.Column<int>(type: "int", nullable: false),
                    Supplier = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    quantity = table.Column<int>(type: "int", nullable: false),
                    purchaseDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_purchases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_purchases_items_item_id",
                        column: x => x.item_id,
                        principalTable: "items",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "saleItemModels",
                columns: table => new
                {
                    SalesItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    sale_id = table.Column<int>(type: "int", nullable: false),
                    item_id = table.Column<int>(type: "int", nullable: false),
                    sale_price = table.Column<double>(type: "float", nullable: false),
                    sale_quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_saleItemModels", x => x.SalesItemId);
                    table.ForeignKey(
                        name: "FK_saleItemModels_items_item_id",
                        column: x => x.item_id,
                        principalTable: "items",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_saleItemModels_sales_sale_id",
                        column: x => x.sale_id,
                        principalTable: "sales",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_purchases_item_id",
                table: "purchases",
                column: "item_id");

            migrationBuilder.CreateIndex(
                name: "IX_saleItemModels_item_id",
                table: "saleItemModels",
                column: "item_id");

            migrationBuilder.CreateIndex(
                name: "IX_saleItemModels_sale_id",
                table: "saleItemModels",
                column: "sale_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "purchases");

            migrationBuilder.DropTable(
                name: "saleItemModels");

            migrationBuilder.DropTable(
                name: "items");

            migrationBuilder.DropTable(
                name: "sales");
        }
    }
}
