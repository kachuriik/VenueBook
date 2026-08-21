using Microsoft.AspNetCore.Mvc;
using VenueBook.Application.DTOs;
using VenueBook.Application.Interfaces;
using VenueBook.Domain.Entities;
using VenueBook.Domain.Services;

namespace VenueBook.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookingsController : ControllerBase
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IRoomRepository _roomRepository;
    private readonly IServiceRepository _serviceRepository;
    private readonly IPricingCalculator _pricingCalculator;

    public BookingsController(
        IBookingRepository bookingRepository,
        IRoomRepository roomRepository,
        IServiceRepository serviceRepository,
        IPricingCalculator pricingCalculator)
    {
        _bookingRepository = bookingRepository;
        _roomRepository = roomRepository;
        _serviceRepository = serviceRepository;
        _pricingCalculator = pricingCalculator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateBooking([FromBody] CreateBookingRequestDto request)
    {
        // 1. Перевірка тривалості
        if (request.DurationHours <= 0)
            return BadRequest("Тривалість бронювання має бути більшою за 0.");

        DateTime endTime = request.StartTime.AddHours(request.DurationHours);

        // 2. Отримуємо зал
        var room = await _roomRepository.GetByIdAsync(request.RoomId);
        if (room == null)
            return NotFound("Зал не знайдено.");

        // 3. Перевірка на перетин часу (чи вільний зал)
        var availableRooms = await _roomRepository.GetAvailableRoomsAsync(request.StartTime, endTime, 0);
        if (!availableRooms.Any(r => r.Id == request.RoomId))
            return Conflict("Зал вже заброньовано на обраний час.");

        // 4. Отримуємо ціни на обрані послуги
        var services = await _serviceRepository.GetServicesByIdsAsync(request.ServiceIds);
        var servicePrices = services.Select(s => s.Price).ToList();

        // 5. Розрахунок вартості (Виклик Domain Service)
        decimal totalPrice = _pricingCalculator.Calculate(
            room.BaseHourlyRate, 
            request.StartTime, 
            endTime, 
            servicePrices);

        // 6. Створення сутності
        var booking = new Booking
        {
            RoomId = room.Id,
            StartTime = request.StartTime,
            EndTime = endTime,
            TotalPrice = totalPrice,
            CreatedAt = DateTime.UtcNow
        };

        // 7. Додаємо послуги із фіксацією ціни
        foreach (var service in services)
        {
            booking.BookingServices.Add(new BookingService 
            { 
                BookingId = booking.Id, 
                ServiceId = service.Id, 
                PriceAtBooking = service.Price 
            });
        }

        // 8. Збереження
        await _bookingRepository.AddAsync(booking);

        // 9. Відповідь
        var response = new BookingResponseDto(
            booking.Id,
            booking.RoomId,
            booking.StartTime,
            booking.EndTime,
            booking.TotalPrice,
            "Confirmed"
        );

        return CreatedAtAction(nameof(GetBooking), new { id = booking.Id }, response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetBooking(Guid id)
    {
        var booking = await _bookingRepository.GetByIdAsync(id);
        if (booking == null) return NotFound();

        var response = new BookingResponseDto(
            booking.Id,
            booking.RoomId,
            booking.StartTime,
            booking.EndTime,
            booking.TotalPrice,
            "Confirmed"
        );
        return Ok(response);
    }
}