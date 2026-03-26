using _2380601390_HuynhVanNam.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace _2380601390_HuynhVanNam.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderDetail> OrderDetails => Set<OrderDetail>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Điện thoại" },
                new Category { Id = 2, Name = "Laptop" },
                new Category { Id = 3, Name = "Phụ kiện" }
            );

            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Name = "iPhone 15",
                    Description = "Điện thoại Apple",
                    Price = 22000000,
                    Stock = 10,
                    ImageUrl = "/images/iphone15.jpg",
                    CategoryId = 1
                },
                new Product
                {
                    Id = 2,
                    Name = "Dell XPS 13",
                    Description = "Laptop cao cấp",
                    Price = 32000000,
                    Stock = 5,
                    ImageUrl = "/images/dellxps13.jpg",
                    CategoryId = 2
                },
                new Product
                {
                    Id = 3,
                    Name = "Tai nghe Bluetooth",
                    Description = "Phụ kiện âm thanh",
                    Price = 1500000,
                    Stock = 20,
                    ImageUrl = "/images/headphone.jpg",
                    CategoryId = 3
                }
            );
        }
    }
}
