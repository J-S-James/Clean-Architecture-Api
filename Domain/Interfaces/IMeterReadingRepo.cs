using Domain.Entities;

namespace Domain.Interfaces;
public interface IMeterReadingRepo
{
    Task<MeterReading?> GetAsync(MeterReading meterReading);
    Task<MeterReading?> GetLatestAsync(MeterReading meterReading);
    Task<int> CreateAsync(MeterReading meterReading);
}
