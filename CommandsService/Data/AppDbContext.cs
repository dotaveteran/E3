using CommandsService.Models;
using Microsoft.EntityFrameworkCore;

namespace CommandsService.Data 
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opt ) : base(opt)
        {
            
        }

        public DbSet<Shkola> Shkolas { get; set; }
        public DbSet<Command> Commands { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Shkola>()
                .HasMany(p => p.Commands)
                .WithOne(p => p.Shkola!)
                .HasForeignKey(p => p.ShkolaId);
            modelBuilder
                .Entity<Command>()
                .HasOne(p => p.Shkola)
                .WithMany(p => p.Commands)
                .HasForeignKey(p => p.ShkolaId);
        }

    }
}