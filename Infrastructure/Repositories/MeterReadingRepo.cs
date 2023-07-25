using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;
public class MeterReadingRepo : IMeterReadingRepo
{
    private readonly IAppDbContext _appDbContext;

    public MeterReadingRepo(IAppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<int> CreateAsync(MeterReading meterReading)
    {
        await _appDbContext.MeterReading
                           .AddAsync(meterReading);

        return await _appDbContext.SaveChangesAsync();
    }

    public async Task<MeterReading?> GetAsync(MeterReading meterReading)
    {
        return await _appDbContext.MeterReading
                            .Where(mr => mr.AccountId == meterReading.AccountId && mr.Date == meterReading.Date)
                            .SingleOrDefaultAsync();
    }

    public async Task<MeterReading?> GetLatestAsync(MeterReading meterReading)
    {
        return await _appDbContext.MeterReading
                                  .Where(mr => mr.AccountId == meterReading.AccountId
                                            && mr.Date == _appDbContext.MeterReading
                                            .Where(innerMR => innerMR.AccountId == meterReading.AccountId)
                                            .Max(innerMR => innerMR.Date))
                                            .SingleOrDefaultAsync();
    }
}