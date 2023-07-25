using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;
public class AppDbContext : DbContext, IAppDbContext
{
    public DbSet<MeterReading> MeterReading { get; set; }
    public DbSet<Account> Account { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MeterReading>()
            .HasNoKey();
    }

    public Task<int> SaveChangesAsync() => base.SaveChangesAsync();
}