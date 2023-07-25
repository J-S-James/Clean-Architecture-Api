using Application.Services;

namespace Services;

[TestFixture]
public class MeterReadingServiceTests
{
    private Mock<IMeterReadingValidator> _validatorMock;
    private Mock<IMeterReadingRepo> _repoMock;
    private IMeterReadingService _meterReadingService;
    private IEnumerable<MeterReading> _meterReadings;

    [SetUp]
    public void Setup()
    {
        _validatorMock = new Mock<IMeterReadingValidator>();
        _repoMock = new Mock<IMeterReadingRepo>();
        _meterReadingService = new MeterReadingService(_validatorMock.Object, _repoMock.Object);
        _meterReadings = new List<MeterReading>
        {
            new MeterReading { AccountId = 1, Date = new DateTime(2023, 7, 12), Value = 100 },
            new MeterReading { AccountId = 2, Date = new DateTime(2023, 7, 12), Value = 200 }
        };
    }

    [Test]
    public async Task ProcessMeterReadingsAsync_ShouldIncrementSuccessfulReadings_WhenValidReadingsProvided()
    {
        // Arrange
        _validatorMock.Setup(v => v.ValidateMeterReading(It.IsAny<MeterReading>()))
             .ReturnsAsync((MeterReading meterReading) => new MeterReadingValidationResult { MeterReading = meterReading });

        _repoMock.Setup(r => r.CreateAsync(It.IsAny<MeterReading>()))
                .ReturnsAsync(1);

        // Act
        var result = await _meterReadingService.ProcessMeterReadingsAsync(_meterReadings);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.SuccessfulReadings, Is.EqualTo(_meterReadings.Count()));
            Assert.That(result.FailedReadings, Is.EqualTo(0));
            Assert.That(result.ErrorMessages, Is.Empty);
        });
    }

    [Test]
    public async Task ProcessMeterReadingsAsync_ShouldIncrementFailedReadingsAndAddErrorMessages_WhenInvalidReadingsProvided()
    {
        // Arrange
        _validatorMock.Setup(v => v.ValidateMeterReading(It.IsAny<MeterReading>()))
                     .ReturnsAsync(new MeterReadingValidationResult { ErrorMessage = "Invalid reading" });

        _repoMock.Setup(r => r.CreateAsync(It.IsAny<MeterReading>()))
                .ReturnsAsync(1);

        // Act
        var result = await _meterReadingService.ProcessMeterReadingsAsync(_meterReadings);

        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.SuccessfulReadings, Is.EqualTo(0));
            Assert.That(result.FailedReadings, Is.EqualTo(_meterReadings.Count()));
            Assert.That(result.ErrorMessages.Count, Is.EqualTo(_meterReadings.Count()));
        });
    }

    [Test]
    public async Task ProcessMeterReadingsAsync_ShouldIncrementFailedReadingsAndAddErrorMessages_WhenRepositoryInsertionFails()
    {
        // Arrange
        _validatorMock.Setup(v => v.ValidateMeterReading(It.IsAny<MeterReading>()))
             .ReturnsAsync((MeterReading meterReading) => new MeterReadingValidationResult { MeterReading = meterReading });

        _repoMock.Setup(r => r.CreateAsync(It.IsAny<MeterReading>()))
                .ReturnsAsync(0);

        // Act
        var result = await _meterReadingService.ProcessMeterReadingsAsync(_meterReadings);
        
        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.SuccessfulReadings, Is.EqualTo(0));
            Assert.That(result.FailedReadings, Is.EqualTo(_meterReadings.Count()));
            Assert.That(result.ErrorMessages.Count, Is.EqualTo(_meterReadings.Count()));
        });
    }
}
