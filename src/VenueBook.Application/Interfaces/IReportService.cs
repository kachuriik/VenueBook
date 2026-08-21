using VenueBook.Application.DTOs;

namespace VenueBook.Application.Interfaces;

public interface IReportService
{
    Task<IEnumerable<RoomOccupancyDto>> GetOccupancyReportAsync(DateTime from, DateTime to);
    Task<RevenueReportDto> GetRevenueReportAsync(DateTime from, DateTime to);
    Task<IEnumerable<PopularServiceDto>> GetPopularServicesAsync(DateTime from, DateTime to);
}