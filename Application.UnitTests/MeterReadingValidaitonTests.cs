using Application.Validation;

namespace Validation;

[TestFixture]
public class MeterReadingValidatorTests
{
    private MeterReadingValidator _meterReadingValidator;
    private Mock<IMeterReadingRepo> _meterReadingRepoMock;
    private Mock<IAccountRepo> _accountRepoMock;
    private MeterReading _meterReading;

    [SetUp]
    public void Setup()
    {
        _meterReadingRepoMock = new Mock<IMeterReadingRepo>();
        _accountRepoMock = new Mock<IAccountRepo>();
        _meterReadingValidator = new MeterReadingValidator(_meterReadingRepoMock.Object, _accountRepoMock.Object);

        _meterReading = new MeterReading
        {
            AccountId = 1,
            Date = new DateTime(2023, 7, 12),
            Value = 500
        };
    }

    [Test]
    public async Task ValidateMeterReading_ShouldReturnValidResult_WhenMeterReadingIsValid()
    {
        //Arrange
        ValidAccountRepoGet();
        ValidMeterReadingRepoGet();
        ValidMeterReadingRepoGetLatest();

        // Act
        var validationResult = await _meterReadingValidator.ValidateMeterReading(_meterReading);

        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(validationResult.MeterReading, Is.EqualTo(_meterReading));
            Assert.That(validationResult.ErrorMessage, Is.Null);
        });
    }

    [Test]
    public async Task ValidateMeterReading_ShouldReturnErrorMessage_WhenAccountNotFound()
    {
        // Arrange
        InvalidAccountRepoGet();
        ValidMeterReadingRepoGet();
        ValidMeterReadingRepoGetLatest();

        // Act
        var validationResult = await _meterReadingValidator.ValidateMeterReading(_meterReading);

        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(validationResult.MeterReading, Is.Null);
            Assert.That(validationResult.ErrorMessage, Is.EqualTo($"Invalid AccountId: {_meterReading.AccountId}. Account does not exist."));
        });
    }

    [Test]
    public async Task ValidateMeterReading_ShouldReturnErrorMessage_WhenDuplicateEntryExists()
    {
        // Arrange
        ValidAccountRepoGet();
        ValidMeterReadingRepoGet();
        DuplicateMeterReadingRepoGetLatest();

        // Act
        var validationResult = await _meterReadingValidator.ValidateMeterReading(_meterReading);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(validationResult.MeterReading, Is.Null);
            Assert.That(validationResult.ErrorMessage, Is.EqualTo($"Duplicate entry for AccountId: {_meterReading.AccountId}, ReadDate: {_meterReading.Date}. Entry already exists."));
        });
    }

    [Test]
    public async Task ValidateMeterReading_ShouldReturnErrorMessage_WhenReadDateIsOlderThanLatestReadDate()
    {
        // Arrange
        ValidAccountRepoGet();
        ValidMeterReadingRepoGet();
        OlderMeterReadingRepoGetLatest();

        // Act
        var validationResult = await _meterReadingValidator.ValidateMeterReading(_meterReading);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(validationResult.MeterReading, Is.Null);
            Assert.That(validationResult.ErrorMessage, Is.EqualTo($"Invalid ReadDate: {_meterReading.Date}. It is older than the newest ReadDate value for AccountId: {_meterReading.AccountId}."));
        });
    }

    [Test]
    public async Task ValidateMeterReading_ShouldReturnErrorMessage_WhenReadValueIsOutOfRange()
    {
        // Arrange
        ValidAccountRepoGet();
        ValidMeterReadingRepoGet();
        ValidMeterReadingRepoGetLatest();
        var meterReading = new MeterReading
        {
            AccountId = _meterReading.AccountId,
            Date = _meterReading.Date,
            Value = 100000
        };

        // Act
        var validationResult = await _meterReadingValidator.ValidateMeterReading(meterReading);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(validationResult.MeterReading, Is.Null);
            Assert.That(validationResult.ErrorMessage, Is.EqualTo($"Invalid ReadValue: {meterReading.Value}. It should be between 0 and 99999."));
        });
    }

    private void ValidAccountRepoGet()
    {
        _accountRepoMock.Setup(r => r.GetAsync(It.IsAny<Account>()))
                        .ReturnsAsync(new Account { Id = _meterReading.AccountId });
    }

    private void InvalidAccountRepoGet()
    {
        _accountRepoMock.Setup(r => r.GetAsync(It.IsAny<Account>()))
                        .ReturnsAsync(null as Account);
    }

    private void ValidMeterReadingRepoGet()
    {
        _meterReadingRepoMock.Setup(r => r.GetAsync(It.IsAny<MeterReading>()))
                             .ReturnsAsync(null as MeterReading);
    }

    private void ValidMeterReadingRepoGetLatest()
    {
        _meterReadingRepoMock.Setup(r => r.GetLatestAsync(It.IsAny<MeterReading>()))
                             .ReturnsAsync((MeterReading meterReading) =>
                             {
                                 return new MeterReading
                                 {
                                     AccountId = meterReading.AccountId,
                                     Date = meterReading.Date.AddMonths(-1),
                                     Value = meterReading.Value
                                 };
                             });
    }

    private void OlderMeterReadingRepoGetLatest()
    {
        _meterReadingRepoMock.Setup(r => r.GetLatestAsync(It.IsAny<MeterReading>()))
                             .ReturnsAsync((MeterReading meterReading) =>
                             {
                                 return new MeterReading
                                 {
                                     AccountId = meterReading.AccountId,
                                     Date = meterReading.Date.AddMonths(1),
                                     Value = meterReading.Value
                                 };
                             });
    }

    private void DuplicateMeterReadingRepoGetLatest()
    {
        _meterReadingRepoMock.Setup(r => r.GetLatestAsync(It.IsAny<MeterReading>()))
                             .ReturnsAsync((MeterReading meterReading) =>
                             {
                                 return meterReading;
                             });
    }
}