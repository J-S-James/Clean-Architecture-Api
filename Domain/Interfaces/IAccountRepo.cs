using Domain.Entities;

namespace Domain.Interfaces;
public interface IAccountRepo
{
    Task<Account?> GetAsync(Account account);
}
