using Microsoft.EntityFrameworkCore;

namespace CommandService.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> context): base(context)
    {
        
    }
    public DbSet<Platform> Platforms{get;set;}
    public DbSet<Command> Commands{get;set;}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Platform>()
        .HasMany<Command>()
        .WithOne(p=>p.Platform)
        .HasForeignKey(p=>p.PlatformId);

        modelBuilder.Entity<Command>()
        .HasOne(p=>p.Platform)
        .WithMany(p=>p.Commands)
        .HasForeignKey(p=>p.PlatformId);

        base.OnModelCreating(modelBuilder);
    }

}
