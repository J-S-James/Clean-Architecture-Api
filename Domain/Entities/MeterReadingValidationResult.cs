namespace Domain.Entities;
public class MeterReadingValidationResult
{
    public MeterReading? MeterReading { get; set; }
    public string? ErrorMessage { get; set; }
}