using Microsoft.EntityFrameworkCore;
using VenueBook.Application.DTOs;
using VenueBook.Application.Interfaces;
using VenueBook.Infrastructure.Data;

namespace VenueBook.Infrastructure.Repositories;

public class ReportService : IReportService
{
    private readonly VenueBookDbContext _context;

    public ReportService(VenueBookDbContext context) => _context = context;

    public async Task<IEnumerable<RoomOccupancyDto>> GetOccupancyReportAsync(DateTime from, DateTime to)
    {
        var bookings = await _context.Bookings
            .Include(b => b.Room)
            .Where(b => b.StartTime >= from && b.EndTime <= to && !b.Room.IsDeleted)
            .ToListAsync();

        var report = bookings
            .GroupBy(b => b.Room.Name)
            .Select(g => new RoomOccupancyDto(
                RoomName: g.Key,
                TotalBookedHours: g.Sum(b => (b.EndTime - b.StartTime).TotalHours)
            ))
            .OrderByDescending(r => r.TotalBookedHours);

        return report;
    }

    public async Task<RevenueReportDto> GetRevenueReportAsync(DateTime from, DateTime to)
    {
        var totalRevenue = await _context.Bookings
            .Where(b => b.StartTime >= from && b.EndTime <= to)
            .SumAsync(b => b.TotalPrice);

        return new RevenueReportDto(totalRevenue);
    }

    public async Task<IEnumerable<PopularServiceDto>> GetPopularServicesAsync(DateTime from, DateTime to)
    {
        var popularServices = await _context.BookingServices
            .Include(bs => bs.Service)
            .Include(bs => bs.Booking)
            .Where(bs => bs.Booking.StartTime >= from && bs.Booking.EndTime <= to)
            .GroupBy(bs => bs.Service.Name)
            .Select(g => new PopularServiceDto(
                g.Key,
                g.Count()
            ))
            .OrderByDescending(s => s.OrderCount)
            .ToListAsync();

        return popularServices;
    }
}