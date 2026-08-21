using VenueBook.Domain.Entities;

namespace VenueBook.Application.Interfaces;

public interface IServiceRepository
{
    // Отримуємо послуги за їхніми ID для фіксації цін під час бронювання
    Task<List<Service>> GetServicesByIdsAsync(IEnumerable<Guid> serviceIds);
}