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

    [HttpGet("occupancy")]
    public async Task<IActionResult> GetOccupancy([FromQuery] DateTime from, [FromQuery] DateTime to)
    {
        var report = await _reportService.GetOccupancyReportAsync(from, to);
        return Ok(report);
    }

    [HttpGet("revenue")]
    public async Task<IActionResult> GetRevenue([FromQuery] DateTime from, [FromQuery] DateTime to)
    {
        var report = await _reportService.GetRevenueReportAsync(from, to);
        return Ok(report);
    }

    [HttpGet("popular-services")]
    public async Task<IActionResult> GetPopularServices([FromQuery] DateTime from, [FromQuery] DateTime to)
    {
        var report = await _reportService.GetPopularServicesAsync(from, to);
        return Ok(report);
    }
}