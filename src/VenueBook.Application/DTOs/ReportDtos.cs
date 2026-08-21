namespace VenueBook.Application.DTOs;

public record RoomOccupancyDto(string RoomName, double TotalBookedHours);

public record RevenueReportDto(decimal TotalRevenue);

public record PopularServiceDto(string ServiceName, int OrderCount);