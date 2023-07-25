using Domain.Entities;

namespace Domain.Interfaces;
public interface IMeterReadingValidator
{
    Task<MeterReadingValidationResult> ValidateMeterReading(MeterReading meterReading);
}