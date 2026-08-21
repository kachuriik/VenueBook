using Microsoft.EntityFrameworkCore;
using VenueBook.Application.Interfaces;
using VenueBook.Domain.Entities;
using VenueBook.Infrastructure.Data;

namespace VenueBook.Infrastructure.Repositories;

public class BookingRepository : IBookingRepository
{
    private readonly VenueBookDbContext _context;

    public BookingRepository(VenueBookDbContext context) => _context = context;

    public async Task<Booking?> GetByIdAsync(Guid id)
    {
        return await _context.Bookings
            .Include(b => b.Room)
            .Include(b => b.BookingServices)
            .ThenInclude(bs => bs.Service)
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task AddAsync(Booking booking)
    {
        await _context.Bookings.AddAsync(booking);
        await _context.SaveChangesAsync();
    }
}