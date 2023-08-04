using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Moq;
using Moq.EntityFrameworkCore;

namespace InfrastructureUnitTests;
public class MeterReadingRepoTests
{
    private Mock<IAppDbContext> _dbContextMock;
    private MeterReadingRepo _meterReadingRepo;
    private List<MeterReading> _meterReadingList;

    [SetUp]
    public void Setup()
    {
        _meterReadingList = new List<MeterReading>
        {
            new MeterReading { AccountId = 1, Date = new DateTime(2023, 7, 12), Value = 100 },
            new MeterReading { AccountId = 1, Date = new DateTime(2023, 8, 12), Value = 300 },
            new MeterReading { AccountId = 2, Date = new DateTime(2023, 7, 12), Value = 200 }
        };

        _dbContextMock = new Mock<IAppDbContext>();
        _dbContextMock.Setup(m => m.MeterReading).ReturnsDbSet(_meterReadingList);

        _meterReadingRepo = new MeterReadingRepo(_dbContextMock.Object);
    }

    [Test]
    public async Task CreateAsync_ShouldReturnIntValue1_WhenSuccessfulCreation()
    {
        var meterReading = new MeterReading { AccountId = 3, Date = new DateTime(2023, 7, 12), Value = 300 };
        _dbContextMock.Setup(m => m.SaveChangesAsync()).ReturnsAsync(1);

        var result = await _meterReadingRepo.CreateAsync(meterReading);

        Assert.That(result, Is.EqualTo(1));
    }

    [Test]
    public async Task CreateAsync_ShouldReturnIntValue0_WheFailedCreation()
    {
        var meterReading = new MeterReading { AccountId = 1, Date = new DateTime(2023, 7, 12), Value = 100 };
        _dbContextMock.Setup(m => m.SaveChangesAsync()).ReturnsAsync(0);

        var result = await _meterReadingRepo.CreateAsync(meterReading);

        Assert.That(result, Is.EqualTo(0));
    }

    [Test]
    public async Task GetAsync_ShouldReturnMeterReadingWithSameAccountIdAndDate_WhenMeterReadingInDb()
    {
        var meterReading = new MeterReading { AccountId = 1, Date = new DateTime(2023, 7, 12) };

        var result = await _meterReadingRepo.GetAsync(meterReading);

        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.AccountId, Is.EqualTo(meterReading.AccountId));
            Assert.That(result.Date, Is.EqualTo(meterReading.Date));
        });
    }

    [Test]
    public async Task GetAsync_ShouldReturnNull_WhenMeterReadingNotInDb()
    {
        var meterReading = new MeterReading { AccountId = 3, Date = new DateTime(2023, 7, 12) };

        var result = await _meterReadingRepo.GetAsync(meterReading);

        Assert.That(result, Is.Null);

    }

    [Test]
    public async Task GetLatestAsync_ShouldReturnMeterReadingWithSameAccountIdAndLatestDate_WhenMeterReadingWithSameAccountIdExistsInDb()
    {
        var meterReading = new MeterReading { AccountId = 1 };
        var expectedResult = GetLatestMeterReading(meterReading.AccountId);



        var result = await _meterReadingRepo.GetLatestAsync(meterReading);

        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.AccountId, Is.EqualTo(meterReading.AccountId));
            Assert.That(result.Date, Is.EqualTo(expectedResult!.Date));
        });
    }

    [Test]
    public async Task GetLatestAsync_ShouldReturnNull_WhenMeterReadingWithSameAccountIdDoesNotExistInDb()
    {
        var meterReading = new MeterReading { AccountId = 3 };
        var expectedResult = GetLatestMeterReading(meterReading.AccountId);



        var result = await _meterReadingRepo.GetLatestAsync(meterReading);

        Assert.Multiple(() =>
        {
            Assert.That(expectedResult, Is.Null);
            Assert.That(result, Is.Null);
        });
    }

    private MeterReading? GetLatestMeterReading(int accountId)
    {
        MeterReading? result = null;

        foreach (MeterReading mr in _meterReadingList)
        {
            if (mr.AccountId == accountId)
            {
                if (result is null || mr.Date > result.Date)
                {
                    result = mr;
                }
            }
        }

        return result;
    }

    [Test]
    public async Task UpdateAsync_ShouldReturnIntValue1_WhenSuccessfulUpdate()
    {
        var meterReading = new MeterReading { AccountId = 1, Date = new DateTime(2023, 7, 12), Value = 200 };
        _dbContextMock.Setup(mr => mr.SaveChangesAsync()).ReturnsAsync(1);

        var result = await _meterReadingRepo.UpdateAsync(meterReading);

        Assert.That(result, Is.EqualTo(1));
    }

    [Test]
    public async Task UpdateAsync_ShouldReturnIntValue1_WhenFailedUpdate()
    {
        var meterReading = new MeterReading { AccountId = 4, Date = new DateTime(2023, 7, 12), Value = 200 };
        _dbContextMock.Setup(mr => mr.SaveChangesAsync()).ReturnsAsync(1);

        var result = await _meterReadingRepo.UpdateAsync(meterReading);

        Assert.That(result, Is.EqualTo(0));
    }

    [Test]
    public async Task DeleteAsync_ShouldReturnIntValue1_WhenSuccessfulDeletion()
    {
        var meterReading = new MeterReading { AccountId = 1, Date = new DateTime(2023, 7, 12) };
        _dbContextMock.Setup(mr => mr.SaveChangesAsync()).ReturnsAsync(1);

        var result = await _meterReadingRepo.DeleteAsync(meterReading);

        Assert.That(result, Is.EqualTo(1));
    }

    [Test]
    public async Task DeleteAsync_ShouldReturnIntValue0_WhenFailedDeletion()
    {
        var meterReading = new MeterReading { AccountId = 4, Date = new DateTime(2023, 7, 12) };
        _dbContextMock.Setup(a => a.SaveChangesAsync()).ReturnsAsync(1);

        var result = await _meterReadingRepo.DeleteAsync(meterReading);

        Assert.That(result, Is.EqualTo(0));
    }
}
