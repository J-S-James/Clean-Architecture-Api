using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;
public class MeterReadingRepo : IMeterReadingRepo
{
    private readonly IAppDbContext _context;

    public MeterReadingRepo(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<int> CreateAsync(MeterReading meterReading)
    {
        await _context.MeterReading
                           .AddAsync(meterReading);

        return await _context.SaveChangesAsync();
    }

    public async Task<MeterReading?> GetAsync(MeterReading meterReading)
    {
        return await _context.MeterReading
                            .Where(mr => mr.AccountId == meterReading.AccountId && mr.Date == meterReading.Date)
                            .SingleOrDefaultAsync();
    }

    public async Task<int> UpdateAsync(MeterReading meterReading)
    {
        var meterReadingDetails = await _context.MeterReading
                             .Where(mr => mr.AccountId == meterReading.AccountId && mr.Date == meterReading.Date)
                             .SingleOrDefaultAsync();

        if (meterReadingDetails is null)
        {
            return 0;
        }
        else
        {
            meterReadingDetails.Value = meterReading.Value;
            return await _context.SaveChangesAsync();
        }
    }

    public async Task<int> DeleteAsync(MeterReading meterReading)
    {
        var meterReadingDetails = await _context.MeterReading
                             .Where(mr => mr.AccountId == meterReading.AccountId && mr.Date == meterReading.Date)
                             .SingleOrDefaultAsync();

        if (meterReadingDetails is null)
        {
            return 0;
        }
        else
        {
            _context.MeterReading.Remove(meterReading);
            return await _context.SaveChangesAsync();
        }
    }

    public async Task<MeterReading?> GetLatestAsync(MeterReading meterReading)
    {
        return await _context.MeterReading
                                  .Where(mr => mr.AccountId == meterReading.AccountId
                                            && mr.Date == _context.MeterReading
                                            .Where(innerMR => innerMR.AccountId == meterReading.AccountId)
                                            .Max(innerMR => innerMR.Date))
                                            .SingleOrDefaultAsync();
    }
}