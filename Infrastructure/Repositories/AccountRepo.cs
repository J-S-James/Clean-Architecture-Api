using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;
public class AccountRepo : IAccountRepo
{
    private readonly IAppDbContext _context;

    public AccountRepo(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<Account?> GetAsync(Account account)
    {
        return await _context.Account
                             .Where(s => s.Id == account.Id)
                             .SingleOrDefaultAsync();
    }

    public async Task<int> CreateAsync(Account account)
    {
        await _context.Account
                      .AddAsync(account);

        return await _context.SaveChangesAsync();
    }

    public async Task<int> UpdateAsync(Account account)
    {
        var accountDetails = await _context.Account
                             .Where(s => s.Id == account.Id)
                             .SingleOrDefaultAsync();

        if (accountDetails is null)
        {
            return 0;
        }
        else
        {
            accountDetails.FirstName = account.FirstName;
            accountDetails.LastName = account.LastName;
            return await _context.SaveChangesAsync();
        }
    }
}