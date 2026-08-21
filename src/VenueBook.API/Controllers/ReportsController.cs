using Microsoft.AspNetCore.Mvc;
using VenueBook.Application.Interfaces;

namespace VenueBook.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportsController : ControllerBase
{
    private readonly IReportService _reportService;

    public ReportsController(IReportService reportService)
    {
        _reportService = reportService;
    }
    /// <summary>
    /// Отримання звіту про завантаженість залів та дохід за обраний період.
    /// </summary>
    /// <param name="from">Початкова дата (UTC)</param>
    /// <param name="to">Кінцева дата (UTC)</param>
    [HttpGet("occupancy")]
    public async Task<IActionResult> GetOccupancy([FromQuery] DateTime from, [FromQuery] DateTime to)
    {
        var report = await _reportService.GetOccupancyReportAsync(from, to);
        return Ok(report);
    }
    /// <summary>
    /// Отримання фінансового звіту про дохід за обраний період.
    /// </summary>
    [HttpGet("revenue")]
    public async Task<IActionResult> GetRevenue([FromQuery] DateTime from, [FromQuery] DateTime to)
    {
        var report = await _reportService.GetRevenueReportAsync(from, to);
        return Ok(report);
    }
    /// <summary>
    /// Отримання статистики популярності додаткових послуг (обладнання, кейтеринг тощо).
    /// </summary>
    [HttpGet("popular-services")]
    public async Task<IActionResult> GetPopularServices([FromQuery] DateTime from, [FromQuery] DateTime to)
    {
        var report = await _reportService.GetPopularServicesAsync(from, to);
        return Ok(report);
    }
}