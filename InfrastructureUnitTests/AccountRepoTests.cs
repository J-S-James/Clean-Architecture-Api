using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Moq;
using Moq.EntityFrameworkCore;

namespace InfrastructureUnitTests;

public class AccountRepoTests
{
    private Mock<IAppDbContext> _dbContextMock;
    private AccountRepo _accountRepo;

    [SetUp]
    public void Setup()
    {
        var sampleAccounts = new List<Account>
            {
                new Account { Id = 1 },
                new Account { Id = 2 },
            };

        _dbContextMock = new Mock<IAppDbContext>();
        _dbContextMock.Setup(m => m.Account).ReturnsDbSet(sampleAccounts);

        _accountRepo = new AccountRepo(_dbContextMock.Object);
    }

    [Test]
    public async Task GetAsync_ShouldReturnAccountWithSameId_WhenAccountIdInDb()
    {
        var account = new Account { Id = 1 };

        var result = await _accountRepo.GetAsync(account);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.EqualTo(account.Id));
    }

    [Test]
    public async Task GetAsync_ShouldReturnNull_WhenAccountIdNotInDb()
    {
        var account = new Account { Id = 3 };

        var result = await _accountRepo.GetAsync(account);

        Assert.That(result, Is.Null);
    }
}