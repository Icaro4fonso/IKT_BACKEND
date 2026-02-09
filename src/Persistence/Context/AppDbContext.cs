using IKT_BACKEND.Persistence.Models;
using Microsoft.EntityFrameworkCore;

namespace IKT_BACKEND.Persistence.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.HasPostgresEnum<PaymentType>();

            modelBuilder.Entity<Sale>(entity =>
            {
                entity.HasKey(s => new { s.DateTime, s.ProductId, s.Price });

                entity.Property(s => s.Price)
                      .HasColumnType("numeric(18,2)");

                entity.HasOne(s => s.Product)
                      .WithMany(p => p.Sales)
                      .HasForeignKey(s => s.ProductId);
            });
        }

        public DbSet<Sale> Sales { get; set; }
        public DbSet<Product> Products { get; set; }
    }
}
