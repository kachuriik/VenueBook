using Microsoft.EntityFrameworkCore;
using VenueBook.Application.Interfaces;
using VenueBook.Domain.Entities;
using VenueBook.Infrastructure.Data;

namespace VenueBook.Infrastructure.Repositories;

public class ServiceRepository : IServiceRepository
{
    private readonly VenueBookDbContext _context;

    public ServiceRepository(VenueBookDbContext context) => _context = context;

    public async Task<List<Service>> GetServicesByIdsAsync(IEnumerable<Guid> serviceIds)
    {
        return await _context.Services
            .Where(s => serviceIds.Contains(s.Id))
            .ToListAsync();
    }
}