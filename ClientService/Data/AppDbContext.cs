using Microsoft.EntityFrameworkCore;
using ReclamationService.Models;

namespace ReclamationService.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Reclamation> Reclamations { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>()
                .HasKey(c => c.Id);

            modelBuilder.Entity<Reclamation>()
                .HasKey(r => r.Id);

            modelBuilder.Entity<Client>()
                .HasMany(c => c.Reclamations)
                .WithOne(r => r.Client)
                .HasForeignKey(r => r.ClientId);
        }
    }
}
