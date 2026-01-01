using InterventionService.Models;
using Microsoft.EntityFrameworkCore;

namespace InterventionService.Data
{
    public class InterventionDbContext : DbContext
    {
        public InterventionDbContext(DbContextOptions<InterventionDbContext> options)
            : base(options) { }

        public DbSet<Intervention> Interventions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Intervention>(entity =>
            {
                // 🔹 Coût des pièces
                entity.Property(e => e.CoutPieces)
                      .HasPrecision(18, 2);

                // 🔹 Coût main d'œuvre
                entity.Property(e => e.CoutMainOeuvre)
                      .HasPrecision(18, 2);

                // 🔹 Montant total facture
                entity.Property(e => e.MontantFacture)
                      .HasPrecision(18, 2);
            });
        }
    }
}
