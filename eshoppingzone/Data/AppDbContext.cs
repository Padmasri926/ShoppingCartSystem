using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using EshoppingZone.Models;

namespace EshoppingZone.Data
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<PaymentTransaction> PaymentTransactions { get; set; }
        public DbSet<Wallet> Wallets { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Order>().Property(o => o.TotalAmount).HasPrecision(18, 2);
            builder.Entity<OrderItem>().Property(o => o.UnitPrice).HasPrecision(18, 2);
            builder.Entity<Payment>().Property(p => p.Amount).HasPrecision(18, 2);
            builder.Entity<PaymentTransaction>().Property(p => p.Amount).HasPrecision(18, 2);
            builder.Entity<Product>().Property(p => p.Price).HasPrecision(18, 2);
            builder.Entity<Wallet>().Property(w => w.Balance).HasPrecision(18, 2);
        }
    }
}