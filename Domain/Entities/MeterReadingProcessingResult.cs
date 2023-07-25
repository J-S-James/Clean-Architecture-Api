namespace Domain.Entities;
public class MeterReadingProcessingResult
{
    public int SuccessfulReadings { get; set; }
    public int FailedReadings { get; set; }
    public List<string> ErrorMessages { get; } = new List<string>();
}