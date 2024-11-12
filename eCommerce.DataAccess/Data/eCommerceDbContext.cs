using eCommerce.Models.Models;
using Microsoft.EntityFrameworkCore;

namespace eCommerce.Data
{
    public class eCommerceDbContext : DbContext
    {
        public eCommerceDbContext() { }

        public eCommerceDbContext(DbContextOptions<eCommerceDbContext> options) : base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            ///Database.Migrate();
        }

        public DbSet<Category> Category { get; set; }
        public DbSet<City> City { get; set; }
        public DbSet<Country> Country { get; set; }
        public DbSet<Password> Password { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<Size> Size { get; set; }
        public DbSet<Color> Color { get; set; }
        public DbSet<ProductVariables> ProductVariables { get; set; }
        public DbSet<ProductComment> ProductComment { get; set; }
        public DbSet<State> State { get; set; }
        public DbSet<SubCategory> SubCategory { get; set; }
        public DbSet<SupplierProduct> SupplierProduct { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<ShoppingCart> ShoppingCart { get; set; }
        public DbSet<Address> Address { get; set; }
        public DbSet<PaymentGateway> PaymentGateway { get; set; }
        public DbSet<Razorpay> Razorpay { get; set; }
        public DbSet<EMailService> EMailService { get; set; }
        public DbSet<EMailJS> EMailJS { get; set; }
        public DbSet<SMSService> SMSService { get; set; }
        public DbSet<FAST2SMS> FAST2SMS { get; set; }
        public DbSet<OrderStatus> OrderStatus { get; set; }
        public DbSet<OrderHeader> OrderHeader { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }
        public DbSet<PaymentStatus> PaymentStatus { get; set; }
        public DbSet<PaymentRange> PaymentRange { get; set; }
        public DbSet<Payments> Payments { get; set; }
    }
}
