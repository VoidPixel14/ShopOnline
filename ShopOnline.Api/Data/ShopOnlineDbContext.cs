using Microsoft.EntityFrameworkCore;
using ShopOnline.Api.Entities;

namespace ShopOnline.Api.Data
{
    public class ShopOnlineDbContext:DbContext
    {
        public ShopOnlineDbContext(DbContextOptions<ShopOnlineDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Products
            modelBuilder.Entity<Product>().HasData(new Product
            {
                Id = 1,
                Name = "MeatPie",
                Description = "Savory pastry filled with spiced meat and vegetables",
                ImageURL = "/Images/Pastry/Meatpie.png",
                Price = 450,
                Qty = 100,
                CategoryId = 1

            });
            modelBuilder.Entity<Product>().HasData(new Product
            {
                Id = 2,
                Name = "Donut",
                Description = "Sweet, fried dough, often glazed or topped, delicious",
                ImageURL = "/Images/Pastry/Donut.png",
                Price = 350,
                Qty = 145,
                CategoryId = 1

            });
            modelBuilder.Entity<Product>().HasData(new Product
            {
                Id = 3,
                Name = "Sausage ",
                Description = "Ground meat seasoned and encased, often grilled or fried",
                ImageURL = "/Images/Pastry/Sausage.png",
                Price = 150,
                Qty = 30,
                CategoryId = 1

            });
            modelBuilder.Entity<Product>().HasData(new Product
            {
                Id = 4,
                Name = "Buns",
                Description = "Soft, flaky, buttery buns with sweet or savory fillings",
                ImageURL = "/Images/Pastry/Buns.png",
                Price = 250,
                Qty = 60,
                CategoryId = 1

            });
            modelBuilder.Entity<Product>().HasData(new Product
            {
                Id = 5,
                Name = "EggRoll",
                Description = "Crispy, thin dough roll with savory egg",
                ImageURL = "/Images/Pastry/EggRoll.png",
                Price = 350,
                Qty = 85,
                CategoryId = 1

            });
            modelBuilder.Entity<Product>().HasData(new Product
            {
                Id = 6,
                Name = "Cake",
                Description = "Simple, sweet, soft cake often enjoyed on its own",
                ImageURL = "/Images/Pastry/Cake.png",
                Price = 250,
                Qty = 120,
                CategoryId = 1

            });
            //Drinks Category
            modelBuilder.Entity<Product>().HasData(new Product
            {
                Id = 7,
                Name = "Caprisun - Orange",
                Description = "Orange caprisun drink",
                ImageURL = "/Images/Drinks/Orange.png",
                Price = 350,
                Qty = 200,
                CategoryId = 2

            });
            modelBuilder.Entity<Product>().HasData(new Product
            {
                Id = 8,
                Name = "Caprisun - Apple",
                Description = "Apple caprisun drink",
                ImageURL = "/Images/Drinks/Apple.png",
                Price = 350,
                Qty = 300,
                CategoryId = 2

            });
            modelBuilder.Entity<Product>().HasData(new Product
            {
                Id = 9,
                Name = "Nutrimilk",
                Description = "Nutritious milk drink, often fortified with vitamins and minerals",
                ImageURL = "/Images/Drinks/Nutrimilk.png",
                Price = 400,
                Qty = 20,
                CategoryId = 2

            });
            //Snack Category
            modelBuilder.Entity<Product>().HasData(new Product
            {
                Id = 10,
                Name = "Sausage Roll",
                Description = "Flaky pastry filled with seasoned sausage, tasty and satisfying",
                ImageURL = "/Images/Snack/SausageRoll.png",
                Price = 150,
                Qty = 212,
                CategoryId = 3
            });

            modelBuilder.Entity<Product>().HasData(new Product
            {
                Id = 11,
                Name = "Rite",
                Description = "Flaky pastry filled with seasoned sausage, tasty and satisfying",
                ImageURL = "/Images/Snack/Rite.png",
                Price = 150,
                Qty = 112,
                CategoryId = 3
            });
            modelBuilder.Entity<Product>().HasData(new Product
            {
                Id = 12,
                Name = "Minimie",
                Description = "Delightful chin chin snack",
                ImageURL = "/Images/Snack/Minimie.png",
                Price = 200,
                Qty = 90,
                CategoryId = 3
            });
            //Add users
            modelBuilder.Entity<User>().HasData(new User
            {
                Id = 1,
                UserName = "Kareem"

            });
            modelBuilder.Entity<User>().HasData(new User
            {
                Id = 2,
                UserName = "Daniel"

            });

            //Create Shopping Cart for Users
            modelBuilder.Entity<Cart>().HasData(new Cart
            {
                Id = 1,
                UserId = 1

            });
            modelBuilder.Entity<Cart>().HasData(new Cart
            {
                Id = 2,
                UserId = 2

            });
            //Add Product Categories
            modelBuilder.Entity<ProductCategory>().HasData(new ProductCategory
            {
                Id = 1,
                Name = "Pastry"
            });
            modelBuilder.Entity<ProductCategory>().HasData(new ProductCategory
            {
                Id = 2,
                Name = "Drink"
            });
            modelBuilder.Entity<ProductCategory>().HasData(new ProductCategory
            {
                Id = 3,
                Name = "Snack"
            });
        }

        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
