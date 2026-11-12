using ClaimBack.API.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ClaimBack.API.Data
{
    public class ClaimBackDbContext : IdentityDbContext<ApplicationUser>
    {
        public ClaimBackDbContext(DbContextOptions<ClaimBackDbContext> options)
            : base(options)
        {

        }
        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<PurchaseDocument> PurchaseDocuments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Purchase>()
                .Property(p => p.PurchasePrice)
                .HasPrecision(18, 2);
        }
    }
}