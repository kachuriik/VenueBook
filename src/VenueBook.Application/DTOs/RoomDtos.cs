namespace VenueBook.Application.DTOs;

public record RoomResponseDto(
    Guid Id, 
    string Name, 
    int Capacity, 
    decimal BaseHourlyRate);

public record CreateRoomRequestDto(
    string Name, 
    int Capacity, 
    decimal BaseHourlyRate, 
    List<Guid> ServiceIds);

public record UpdateRoomRequestDto(
    string Name, 
    int Capacity, 
    decimal BaseHourlyRate, 
    List<Guid> ServiceIds);