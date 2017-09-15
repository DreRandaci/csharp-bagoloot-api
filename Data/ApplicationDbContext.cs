using Microsoft.EntityFrameworkCore;
using BagoLootAPI.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace BagoLootAPI.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }

        public DbSet<Toy> Toy { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Child> Child { get; set; }
        public DbSet<Reindeer> Reindeer { get; set; }
        public DbSet<FavoriteReindeer> FavoriteReindeer { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}