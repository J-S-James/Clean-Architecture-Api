using CsvHelper;
using CsvHelper.Configuration;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Presentation.Controllers;
using System.Globalization;
using System.Text;
using System.Text.Json;

namespace Presentation.UnitTests;
public class MeterReadingControllerTests
{
    private Mock<IMeterReadingService> _meterReadingServiceMock;
    private MeterReadingController _meterReadingController;
    private List<MeterReading> _meterReadings;

    [SetUp]
    public void Setup()
    {
        _meterReadingServiceMock = new Mock<IMeterReadingService>();
        _meterReadingController = new MeterReadingController(_meterReadingServiceMock.Object);
        _meterReadings = new List<MeterReading>
        {
            new MeterReading { AccountId = 1, Date = new DateTime(2023, 7, 12), Value = 100 },
            new MeterReading { AccountId = 2, Date = new DateTime(2023, 7, 12), Value = 200 }
        };

        _meterReadingServiceMock
            .Setup(x => x.ProcessMeterReadingsAsync(It.IsAny<List<MeterReading>>()))
            .Returns(Task.FromResult(new MeterReadingProcessingResult()));
    }

    [Test]
    public async Task CreateMrAsync_ShouldProcessMeterReadings_WhenGivenValidFormFile()
    {
        var formFile = CreateCsvFormFile(_meterReadings);

        var result = await _meterReadingController.CreateMrAsync(formFile);

        Assert.That(result, Is.TypeOf<OkObjectResult>());
    }

    [Test]
    public async Task CreateMrAsync_ShouldReturnBadRequest_WhenGivenEmptyFormFile()
    {
        var formFile = new FormFile(new MemoryStream(), 0, 0, "emptyfile", "emptyfile.csv");

        var result = await _meterReadingController.CreateMrAsync(formFile);

        Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
        var badRequestResult = (BadRequestObjectResult)result;
        Assert.That(badRequestResult.Value, Is.EqualTo("No File Uploaded or file is empty."));
    }

    [Test]
    public async Task CreateMrAsync_ShouldReturnBadRequest_WhenGivenNull()
    {
        var result = await _meterReadingController.CreateMrAsync(null!);

        Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
        var badRequestResult = (BadRequestObjectResult)result;
        Assert.That(badRequestResult.Value, Is.EqualTo("No File Uploaded or file is empty."));
    }

    [Test]
    public async Task CreateMrAsync_ShouldThrowException_WhenGivenInvalidFile()
    {
        var formFile = CreateJsonFormFile(_meterReadings);

        var result = await _meterReadingController.CreateMrAsync(formFile);

        Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
    }

    private static IFormFile CreateCsvFormFile(List<MeterReading> data)
    {
        var csvData = new StringBuilder();

        var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture);
        using (var csvWriter = new CsvWriter(new StringWriter(csvData), csvConfig))
        {
            csvWriter.WriteRecords(data);
        }

        var csvBytes = Encoding.UTF8.GetBytes(csvData.ToString());

        var stream = new MemoryStream(csvBytes);

        var file = new FormFile(stream, 0, stream.Length, "testData", "test_data.csv");

        return file;
    }

    private IFormFile CreateJsonFormFile(List<MeterReading> data)
    {
        var jsonData = JsonSerializer.Serialize(data);

        var jsonBytes = Encoding.UTF8.GetBytes(jsonData);

        var stream = new MemoryStream(jsonBytes);

        var file = new FormFile(stream, 0, stream.Length, "meterReadings", "meter_readings.json");

        return file;
    }
}