using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services;
public class MeterReadingService : IMeterReadingService
{
    private readonly IMeterReadingRepo _meterReadingRepo;
    private readonly IMeterReadingValidator _validator;

    public MeterReadingService(IMeterReadingValidator validator, IMeterReadingRepo meterReadingRepo)
    {
        _meterReadingRepo = meterReadingRepo;
        _validator = validator;
    }

    public async Task<MeterReadingProcessingResult> ProcessMeterReadingsAsync(IEnumerable<MeterReading> meterReadings)
    {
        var result = new MeterReadingProcessingResult();

        foreach (var meterReading in meterReadings)
        {
            var validation = await _validator.ValidateMeterReading(meterReading);
            if (validation.MeterReading is null)
            {
                result.FailedReadings++;
                result.ErrorMessages.Add(validation.ErrorMessage!);
                continue;
            }

            var insertResult = await _meterReadingRepo.CreateAsync(meterReading);

            if (insertResult == 0)
            {
                result.FailedReadings++;
                result.ErrorMessages.Add($"Failed to insert {meterReading}");
                continue;
            }
            result.SuccessfulReadings++;
        }

        return result;
    }
}