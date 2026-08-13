namespace VenueBook.Domain.Entities;

public class BookingService
{
    public Guid BookingId { get; set; }
    public Booking Booking { get; set; } = null!;

    public Guid ServiceId { get; set; }
    public Service Service { get; set; } = null!;

    // Фіксація ціни на момент бронювання
    public decimal PriceAtBooking { get; set; }
}