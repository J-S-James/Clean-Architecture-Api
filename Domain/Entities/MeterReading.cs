namespace Domain.Entities;
public class MeterReading
{
    public int AccountId { get; init; }
    public DateTime Date { get; init; }
    public int? Value { get; init; }
}