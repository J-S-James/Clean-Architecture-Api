using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;
public interface IAppDbContext
{
    DbSet<Account> Account { get; set; }
    DbSet<MeterReading> MeterReading { get; set; }

    public Task<int> SaveChangesAsync();
}