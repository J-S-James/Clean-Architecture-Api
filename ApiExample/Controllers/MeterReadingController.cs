using CsvHelper;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MeterReadingController : BaseApiController
{
    private readonly IMeterReadingService _meterReadingService;

    public MeterReadingController(IMeterReadingService meterReadingService)
    {
        _meterReadingService = meterReadingService;
    }

    [HttpPost("/meter-reading-uploads")]
    public async Task<IActionResult> CreateMrAsync(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("No File Uploaded or file is empty.");
        }

        List<MeterReading> meterReadings;

        using var reader = new StreamReader(file.OpenReadStream());
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        try
        {
            meterReadings = csv.GetRecords<MeterReading>().ToList();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

        var result = await _meterReadingService.ProcessMeterReadingsAsync(meterReadings);

        return Ok(result);
    }
}
