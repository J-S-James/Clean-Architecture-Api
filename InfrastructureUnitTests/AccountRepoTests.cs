using Domain.Entities;
using Domain.Interfaces;
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
                new Account { Id = 1, FirstName = "John", LastName = "Doe" },
                new Account { Id = 2, FirstName = "Jane", LastName = "Doe" },
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

    [Test]
    public async Task CreateAsync_ShouldReturnIntValue1_WhenSuccessfulCreation()
    {
        var account = new Account { Id = 3, FirstName = "Jeremy", LastName = "Doe" };
        _dbContextMock.Setup(a => a.SaveChangesAsync()).ReturnsAsync(1);

        var result = await _accountRepo.CreateAsync(account);

        Assert.That(result, Is.EqualTo(1));
    }

    [Test]
    public async Task CreateAsync_ShouldReturnIntValue0_WheFailedCreation()
    {
        var account = new Account { Id = 3, FirstName = "Jeremy", LastName = "Doe" };
        _dbContextMock.Setup(a => a.SaveChangesAsync()).ReturnsAsync(0);

        var result = await _accountRepo.CreateAsync(account);

        Assert.That(result, Is.EqualTo(0));
    }

    [Test]
    public async Task UpdateAsync_ShouldReturnIntValue1_WhenSuccessfulUpdate()
    {
        var account = new Account { Id = 1, FirstName = "Dave", LastName = "Doe" };
        _dbContextMock.Setup(a => a.SaveChangesAsync()).ReturnsAsync(1);

        var result = await _accountRepo.UpdateAsync(account);

        Assert.That(result, Is.EqualTo(1));
    }

    [Test]
    public async Task UpdateAsync_ShouldReturnIntValue1_WhenFailedUpdate()
    {
        var account = new Account { Id = 3, FirstName = "Dave", LastName = "Doe" };
        _dbContextMock.Setup(a => a.SaveChangesAsync()).ReturnsAsync(1);

        var result = await _accountRepo.UpdateAsync(account);

        Assert.That(result, Is.EqualTo(0));
    }
}