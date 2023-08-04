using Application.Services;

namespace Services;
[TestFixture]
public class AccountServiceTest
{
    private Mock<IAccountRepo> _repoMock;
    private IAccountService _accountService;

    [SetUp]
    public void Setup()
    {
        _repoMock = new Mock<IAccountRepo>();
        _accountService = new AccountService(_repoMock.Object);
    }


    [Test]
    public async Task CreateAccountAsync_ShouldReturnInt1_WhenAccountDoesNotAlreadyExist()
    {
        Account? n = null;
        _repoMock.Setup(a => a.GetAsync(It.IsAny<Account>())).ReturnsAsync(n);
        _repoMock.Setup(a => a.CreateAsync(It.IsAny<Account>())).ReturnsAsync(1);

        var result = await _accountService.CreateAccountAsync(new Account { Id = 1 });

        Assert.That(result, Is.EqualTo(1));
    }

    [Test]
    public async Task CreateAccountAsync_ShouldReturnInt0_WhenAccountAlreadyExists()
    {
        _repoMock.Setup(a => a.GetAsync(It.IsAny<Account>())).ReturnsAsync((Account account) => { return account; });

        var result = await _accountService.CreateAccountAsync(new Account { Id = 1 });

        Assert.That(result, Is.EqualTo(0));
    }
}
