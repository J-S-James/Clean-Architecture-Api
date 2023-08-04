using Domain.Entities;

namespace Domain.Interfaces;
public interface IMeterReadingRepo
{
    Task<int> CreateAsync(MeterReading meterReading);
    Task<MeterReading?> GetAsync(MeterReading meterReading);
    Task<int> UpdateAsync(MeterReading meterReading);
    Task<int> DeleteAsync(MeterReading meterReading);
    Task<MeterReading?> GetLatestAsync(MeterReading meterReading);
}
