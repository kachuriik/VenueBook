using Microsoft.EntityFrameworkCore;
using VenueBook.Domain.Entities;

namespace VenueBook.Infrastructure.Data;

public class VenueBookDbContext : DbContext
{
    public VenueBookDbContext(DbContextOptions<VenueBookDbContext> options) : base(options) { }

    public DbSet<Room> Rooms => Set<Room>();
    public DbSet<Service> Services => Set<Service>();
    public DbSet<RoomService> RoomServices => Set<RoomService>();
    public DbSet<Booking> Bookings => Set<Booking>();
    public DbSet<BookingService> BookingServices => Set<BookingService>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // 1. Налаштування зв'язків (Складені ключі для Many-to-Many)
        modelBuilder.Entity<RoomService>()
            .HasKey(rs => new { rs.RoomId, rs.ServiceId });

        modelBuilder.Entity<BookingService>()
            .HasKey(bs => new { bs.BookingId, bs.ServiceId });

        // 2. Статичні GUID для Seeding
        var roomAId = Guid.Parse("11111111-1111-1111-1111-111111111111");
        var roomBId = Guid.Parse("22222222-2222-2222-2222-222222222222");
        var roomCId = Guid.Parse("33333333-3333-3333-3333-333333333333");

        var projectorId = Guid.Parse("44444444-4444-4444-4444-444444444444");
        var wifiId = Guid.Parse("55555555-5555-5555-5555-555555555555");
        var soundId = Guid.Parse("66666666-6666-6666-6666-666666666666");

        // 3. Автозаповнення Залів
        modelBuilder.Entity<Room>().HasData(
            new Room { Id = roomAId, Name = "Зал А", Capacity = 50, BaseHourlyRate = 2000, IsDeleted = false },
            new Room { Id = roomBId, Name = "Зал B", Capacity = 100, BaseHourlyRate = 3500, IsDeleted = false },
            new Room { Id = roomCId, Name = "Зал C", Capacity = 30, BaseHourlyRate = 1500, IsDeleted = false }
        );

        // 4. Автозаповнення Послуг
        modelBuilder.Entity<Service>().HasData(
            new Service { Id = projectorId, Name = "Проєктор", Price = 500 },
            new Service { Id = wifiId, Name = "Wi-Fi", Price = 300 },
            new Service { Id = soundId, Name = "Звук", Price = 700 }
        );
    }
}
