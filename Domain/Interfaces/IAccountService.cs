using Domain.Entities;

namespace Domain.Interfaces;
public interface IAccountService
{
    Task<Account> CreateAccountAsync(Account account);
    Task<Account> GetAccountAsync(Account account);
    Task<Account> UpdateAccountAsync(Account account);
    Task<Account> DeleteAccountAsync(Account account);
}
