using Microsoft.EntityFrameworkCore;
using VenueBook.Application.Interfaces;
using VenueBook.Domain.Entities;
using VenueBook.Infrastructure.Data;

namespace VenueBook.Infrastructure.Repositories;

public class RoomRepository : IRoomRepository
{
    private readonly VenueBookDbContext _context;

    public RoomRepository(VenueBookDbContext context)
    {
        _context = context;
    }

    public async Task<Room?> GetByIdAsync(Guid id)
    {
        return await _context.Rooms
            .Include(r => r.RoomServices)
            .ThenInclude(rs => rs.Service)
            .FirstOrDefaultAsync(r => r.Id == id && !r.IsDeleted);
    }

    public async Task<IEnumerable<Room>> GetAllAsync()
    {
        return await _context.Rooms
            .Where(r => !r.IsDeleted)
            .Include(r => r.RoomServices)
            .ThenInclude(rs => rs.Service)
            .ToListAsync();
    }

    public async Task<IEnumerable<Room>> GetAvailableRoomsAsync(DateTime startTime, DateTime endTime, int minCapacity)
    {
        return await _context.Rooms
            .Where(r => !r.IsDeleted && r.Capacity >= minCapacity)
            .Where(r => !r.Bookings.Any(b => 
                b.StartTime < endTime && b.EndTime > startTime)) 
            .Include(r => r.RoomServices)
            .ThenInclude(rs => rs.Service)
            .ToListAsync();
    }

    public async Task AddAsync(Room room)
    {
        await _context.Rooms.AddAsync(room);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Room room)
    {
        _context.Rooms.Update(room);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var room = await _context.Rooms.FindAsync(id);
        if (room != null)
        {
            room.IsDeleted = true;
            await _context.SaveChangesAsync();
        }
    }
}