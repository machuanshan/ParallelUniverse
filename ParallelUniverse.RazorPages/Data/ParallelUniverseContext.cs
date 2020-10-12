using Microsoft.EntityFrameworkCore;
using ParallelUniverse.RazorPages.Models;

namespace ParallelUniverse.RazorPages.Data
{
    public class ParallelUniverseContext : DbContext
    {
        public ParallelUniverseContext(DbContextOptions options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Guest>().HasIndex(g => g.Name).IsUnique();
            modelBuilder.Entity<FileResource>().HasIndex(f => f.Name);
            modelBuilder.Entity<FileResource>().HasIndex(f => f.GuestId);
        }

        public DbSet<FileResource> FileResource { get; set; }
        public DbSet<Guest> Guest { get; set; }
    }
}
