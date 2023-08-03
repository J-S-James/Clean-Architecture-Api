using Domain.Entities;

namespace Domain.Interfaces;
public interface IAccountRepo
{
    Task<int> CreateAsync(Account account);
    Task<Account?> GetAsync(Account account);
    Task<int> UpdateAsync(Account account);
    Task<int> DeleteAsync(Account account);
}
