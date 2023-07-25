using Domain.Entities;
using Domain.Interfaces;

namespace Application.Validation;
public class MeterReadingValidator : IMeterReadingValidator
{
    private readonly IMeterReadingRepo _meterReadingRepo;
    private readonly IAccountRepo _accountRepo;

    public MeterReadingValidator(IMeterReadingRepo meterReadingDA, IAccountRepo accountDA)
    {
        _meterReadingRepo = meterReadingDA;
        _accountRepo = accountDA;
    }

    public async Task<MeterReadingValidationResult> ValidateMeterReading(MeterReading meterReading)
    {
        var validationResult = new MeterReadingValidationResult();

        var account = await _accountRepo.GetAsync(new Account() { Id = meterReading.AccountId });
        if (account is null)
        {
            validationResult.ErrorMessage = $"Invalid AccountId: {meterReading.AccountId}. Account does not exist.";
            return validationResult;
        }

        var latestReading = await _meterReadingRepo.GetLatestAsync(meterReading);
        if (latestReading is not null)
        {
            if (latestReading.Date == meterReading.Date)
            {
                validationResult.ErrorMessage = $"Duplicate entry for AccountId: {meterReading.AccountId}, ReadDate: {meterReading.Date}. Entry already exists.";
                return validationResult;
            }

            if (latestReading.Date > meterReading.Date)
            {
                validationResult.ErrorMessage = $"Invalid ReadDate: {meterReading.Date}. It is older than the newest ReadDate value for AccountId: {meterReading.AccountId}.";
                return validationResult;
            }
        }

        if (meterReading.Value < 0 || meterReading.Value > 99999)
        {
            validationResult.ErrorMessage = $"Invalid ReadValue: {meterReading.Value}. It should be between 0 and 99999.";
            return validationResult;
        }

        validationResult.MeterReading = meterReading;
        return validationResult;
    }
}