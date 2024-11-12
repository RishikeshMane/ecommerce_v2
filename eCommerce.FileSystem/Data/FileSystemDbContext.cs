using FileSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace FileSystem.Data
{
    public class FileSystemDbContext : DbContext
    {
        public FileSystemDbContext() { }

        public FileSystemDbContext(DbContextOptions<FileSystemDbContext> options) : base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }

        public DbSet<Product> Product { get; set; }
        public DbSet<ProductVariables> ProductVariables { get; set; }
        public DbSet<SupplierProduct> SupplierProduct { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<SubCategory> SubCategory { get; set; }
        public DbSet<City> City { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Size> Size { get; set; }
        public DbSet<Color> Color { get; set; }
        public DbSet<ShoppingCart> ShoppingCart { get; set; }
    }
}
