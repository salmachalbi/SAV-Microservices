using ArticleService.Models;
using Microsoft.EntityFrameworkCore;

using System.Collections.Generic;
using System.Reflection.Emit;

namespace ArticleService.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Article> Articles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Article>(entity =>
            {
                entity.HasKey(a => a.Id);

                entity.Property(a => a.Nom)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(a => a.DateAchat)
                      .IsRequired();

                entity.Property(a => a.GarantieMois)
                      .IsRequired();
            });
        }
    }
}

