using Domain.Entities;

namespace Domain.Interfaces;
public interface IMeterReadingService
{
    Task<MeterReadingProcessingResult> ProcessMeterReadingsAsync(IEnumerable<MeterReading> meterReadings);
}