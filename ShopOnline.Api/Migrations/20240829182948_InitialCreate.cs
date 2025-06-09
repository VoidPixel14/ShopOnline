using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ShopOnline.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CartItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CartId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Qty = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Carts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageURL = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Qty = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Carts",
                columns: new[] { "Id", "UserId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 2 }
                });

            migrationBuilder.InsertData(
                table: "ProductCategories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Pastry" },
                    { 2, "Drink" },
                    { 3, "Snack" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryId", "Description", "ImageURL", "Name", "Price", "Qty" },
                values: new object[,]
                {
                    { 1, 1, "Savory pastry filled with spiced meat and vegetables", "/Images/Pastry/Meatpie.png", "MeatPie", 450m, 100 },
                    { 2, 1, "Sweet, fried dough, often glazed or topped, delicious", "/Images/Pastry/Donut.png", "Donut", 350m, 145 },
                    { 3, 1, "Ground meat seasoned and encased, often grilled or fried", "/Images/Pastry/Sausage.png", "Sausage ", 150m, 30 },
                    { 4, 1, "Soft, flaky, buttery buns with sweet or savory fillings", "/Images/Pastry/Buns.png", "Buns", 250m, 60 },
                    { 5, 1, "Crispy, thin dough roll with savory egg", "/Images/Pastry/EggRoll.png", "EggRoll", 350m, 85 },
                    { 6, 1, "Simple, sweet, soft cake often enjoyed on its own", "/Images/Pastry/Cake.png", "Cake", 250m, 120 },
                    { 7, 2, "Orange caprisun drink", "/Images/Drinks/Orange.png", "Caprisun - Orange", 150m, 200 },
                    { 8, 2, "Apple caprisun drink", "/Images/Drinks/Apple.png", "Caprisun - Apple", 150m, 300 },
                    { 9, 2, "Nutritious milk drink, often fortified with vitamins and minerals", "/Images/Drinks/Nutrimilk.png", "Nutrimilk", 150m, 20 },
                    { 10, 3, "Flaky pastry filled with seasoned sausage, tasty and satisfying", "/Images/Snack/SausageRoll.png", "Sausage Roll", 150m, 212 },
                    { 11, 3, "Flaky pastry filled with seasoned sausage, tasty and satisfying", "/Images/Snack/Rite.png", "Rite", 150m, 112 },
                    { 12, 3, "Delightful chin chin snack", "/Images/Snack/Minimie.png", "Minimie", 200m, 90 }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "UserName" },
                values: new object[,]
                {
                    { 1, "Kareem" },
                    { 2, "Daniel" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CartItems");

            migrationBuilder.DropTable(
                name: "Carts");

            migrationBuilder.DropTable(
                name: "ProductCategories");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
