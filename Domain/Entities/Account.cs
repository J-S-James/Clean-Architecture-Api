namespace Domain.Entities;
public class Account
{
    public int Id { get; init; }
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
}