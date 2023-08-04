using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services;
public class AccountService : IAccountService
{
    public IAccountRepo _accountRepo;

    public AccountService(IAccountRepo accountRepo)
    {
        _accountRepo = accountRepo;
    }

    public async Task<int> CreateAccountAsync(Account account)
    {
        var existingAccount = await _accountRepo.GetAsync(account);

        if (existingAccount is not null)
        {
            return 0;
        }

        return await _accountRepo.CreateAsync(account);
    }

    public async Task<int> DeleteAccountAsync(Account account)
    {
        return await _accountRepo.DeleteAsync(account);
    }

    public async Task<Account?> GetAccountAsync(Account account)
    {
        return await _accountRepo.GetAsync(account);
    }

    public async Task<int> UpdateAccountAsync(Account account)
    {
        return await _accountRepo.UpdateAsync(account);
    }
}
