using Microsoft.EntityFrameworkCore;

namespace ChantingApp.Persistence;

public class ChantsDbContext : DbContext
{
    public ChantsDbContext(DbContextOptions<ChantsDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Chant> Chants => Set<Chant>();
    public DbSet<ChantStream> Streams => Set<ChantStream>();
    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}