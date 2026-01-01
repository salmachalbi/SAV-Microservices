using AuthService.Models;
using AuthService.Services;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Data
{
    public class AuthDbContext : DbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> opts) : base(opts) { }

        public DbSet<User> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var passwordService = new PasswordService();
            passwordService.CreatePasswordHash("Admin123!", out var hash, out var salt);

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Username = "sav_admin",
                    Email = "sav@company.com",
                    PasswordHash = hash,
                    PasswordSalt = salt,
                    Role = "ResponsableSAV"
                }
            );
        }
    }
}
