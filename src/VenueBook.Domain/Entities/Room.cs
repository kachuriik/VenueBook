namespace VenueBook.Domain.Entities;

public class Room
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public int Capacity { get; set; }
    public decimal BaseHourlyRate { get; set; }
    public bool IsDeleted { get; set; } = false;

    public ICollection<RoomService> RoomServices { get; set; } = new List<RoomService>();
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();

}