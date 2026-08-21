namespace VenueBook.Application.DTOs;

public record CreateBookingRequestDto(
    Guid RoomId,
    DateTime StartTime,
    int DurationHours,
    List<Guid> ServiceIds);

public record BookingResponseDto(
    Guid Id,
    Guid RoomId,
    DateTime StartTime,
    DateTime EndTime,
    decimal TotalPrice,
    string Status);