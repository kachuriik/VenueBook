using Microsoft.AspNetCore.Mvc;
using Moq;
using VenueBook.API.Controllers;
using VenueBook.Application.DTOs;
using VenueBook.Application.Interfaces;
using VenueBook.Domain.Entities;

namespace VenueBook.UnitTests.API;

public class RoomsControllerTests
{
    [Fact]
    public async Task GetAll_ShouldReturnOkStatus_WithListOfRoomResponseDtos()
    {
        var mockRepo = new Mock<IRoomRepository>();
        var roomId = Guid.NewGuid();
        mockRepo.Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(new List<Room>
            {
                new Room { Id = roomId, Name = "Conference Hall A", Capacity = 50, BaseHourlyRate = 1000m }
            });

        var controller = new RoomsController(mockRepo.Object);

        var result = await controller.GetAll();
        
        var okResult = Assert.IsType<OkObjectResult>(result);
        
        var returnValue = Assert.IsAssignableFrom<IEnumerable<RoomResponseDto>>(okResult.Value);
        Assert.Single(returnValue);
        Assert.Equal("Conference Hall A", returnValue.First().Name);
    }

    [Fact]
    public async Task Update_ShouldReturnNotFound_WhenRoomDoesNotExist()
    {
        var mockRepo = new Mock<IRoomRepository>();
        var nonExistingId = Guid.NewGuid();
        
        mockRepo.Setup(repo => repo.GetByIdAsync(nonExistingId))
            .ReturnsAsync((Room?)null);

        var controller = new RoomsController(mockRepo.Object);
        var request = new UpdateRoomRequestDto("New Name", 20, 1200m, new List<Guid>());

        var result = await controller.Update(nonExistingId, request);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("Зал не знайдено.", notFoundResult.Value);
    }
}