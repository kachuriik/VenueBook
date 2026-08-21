using VenueBook.Domain.Entities;

namespace VenueBook.Application.Interfaces;

public interface IBookingRepository
{
    Task<Booking?> GetByIdAsync(Guid id);
    Task AddAsync(Booking booking);
}