namespace VenueBook.Domain.Entities;

public class Service
{
    public class Service
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }

        public ICollection<RoomService> RoomServices { get; set; } = new List<RoomService>();
    }
}