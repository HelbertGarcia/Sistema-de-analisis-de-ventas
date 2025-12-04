using Microsoft.EntityFrameworkCore;
using SAV.Domain.Entities.Dwh.Dimentions; // Asegúrate de que este namespace sea correcto según tus carpetas
using SAV.Domain.Entities.Dwh.Facts;

namespace SAV.Persistence.Destination
{
    public class DwhDbContext : DbContext
    {
        public DwhDbContext(DbContextOptions<DwhDbContext> options) : base(options)
        {
        }

        // DbSets
        public DbSet<DimProducts> DimProducts { get; set; }
        public DbSet<DimCustomers> DimCustomers { get; set; }
        public DbSet<DimDate> DimDate { get; set; }
        public DbSet<DimStatus> DimStatus { get; set; }
        public DbSet<FactSales> FactSales { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DimDate>(entity =>
            {
                entity.Property(d => d.DateKey)
                      .ValueGeneratedNever();
            });

            modelBuilder.Entity<FactSales>(entity =>
            {
                entity.Property(f => f.TotalPrice).HasPrecision(18, 2);
                entity.HasOne<DimProducts>().WithMany().HasForeignKey(f => f.ProductKey);
                entity.HasOne<DimCustomers>().WithMany().HasForeignKey(f => f.CustomerKey);
                entity.HasOne<DimDate>().WithMany().HasForeignKey(f => f.DateKey);
                entity.HasOne<DimStatus>().WithMany().HasForeignKey(f => f.StatusKey);
            });
        }
    }
}