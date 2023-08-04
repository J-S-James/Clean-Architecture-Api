using Domain.Entities;

namespace Domain.Interfaces;
public interface IAccountService
{
    Task<int> CreateAccountAsync(Account account);
    Task<Account?> GetAccountAsync(Account account);
    Task<int> UpdateAccountAsync(Account account);
    Task<int> DeleteAccountAsync(Account account);
}
