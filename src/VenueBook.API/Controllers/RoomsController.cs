using Microsoft.AspNetCore.Mvc;
using VenueBook.Application.DTOs;
using VenueBook.Application.Interfaces;
using VenueBook.Domain.Entities;

namespace VenueBook.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoomsController : ControllerBase
{
    private readonly IRoomRepository _roomRepository;

    public RoomsController(IRoomRepository roomRepository)
    {
        _roomRepository = roomRepository;
    }

    // 1. Отримання всіх залів
    /// <summary>
    /// Отримання повного списку конференц-залів.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var rooms = await _roomRepository.GetAllAsync();
        var response = rooms.Select(r => new RoomResponseDto(r.Id, r.Name, r.Capacity, r.BaseHourlyRate));
        return Ok(response);
    }

    
    
    // 2. Пошук доступних залів
    /// <summary>
    /// Пошук доступних залів на заданий проміжок часу та мінімальну місткість.
    /// </summary>
    /// <param name="startTime">Час початку (UTC)</param>
    /// <param name="endTime">Час завершення (UTC)</param>
    /// <param name="capacity">Мінімальна місткість осіб</param>
    [HttpGet("available")]
    public async Task<IActionResult> GetAvailable(
        [FromQuery] DateTime startTime, 
        [FromQuery] DateTime endTime, 
        [FromQuery] int capacity)
    {
        var rooms = await _roomRepository.GetAvailableRoomsAsync(startTime, endTime, capacity);
        var response = rooms.Select(r => new RoomResponseDto(r.Id, r.Name, r.Capacity, r.BaseHourlyRate));
        return Ok(response);
    }

    // 3. Додавання конференц-залу
    /// <summary>
    /// Додавання нового конференц-залу в систему.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateRoomRequestDto request)
    {
        var newRoom = new Room
        {
            Name = request.Name,
            Capacity = request.Capacity,
            BaseHourlyRate = request.BaseHourlyRate
        };

        foreach (var serviceId in request.ServiceIds)
        {
            newRoom.RoomServices.Add(new RoomService { RoomId = newRoom.Id, ServiceId = serviceId });
        }

        await _roomRepository.AddAsync(newRoom);

        return CreatedAtAction(nameof(GetAll), new { id = newRoom.Id }, newRoom.Id);
    }

    // 4. Редагування інформації про зал
    /// <summary>
    /// Оновлення параметрів існуючого конференц-залу.
    /// </summary>
    /// <param name="id">Ідентифікатор залу</param>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateRoomRequestDto request)
    {
        var existingRoom = await _roomRepository.GetByIdAsync(id);
        if (existingRoom == null)
            return NotFound("Зал не знайдено.");

        existingRoom.Name = request.Name;
        existingRoom.Capacity = request.Capacity;
        existingRoom.BaseHourlyRate = request.BaseHourlyRate;
        
        existingRoom.RoomServices.Clear();
        foreach (var serviceId in request.ServiceIds)
        {
            existingRoom.RoomServices.Add(new RoomService { RoomId = existingRoom.Id, ServiceId = serviceId });
        }

        await _roomRepository.UpdateAsync(existingRoom);
        return Ok("Зал успішно оновлено.");
    }

    // 5. Видалення конференц-залу
    /// <summary>
    /// Видалення конференц-залу за ідентифікатором.
    /// </summary>
    /// <param name="id">Ідентифікатор залу</param>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var room = await _roomRepository.GetByIdAsync(id);
        if (room == null)
            return NotFound("Зал не знайдено.");

        await _roomRepository.DeleteAsync(id);
        return Ok("Зал успішно видалено.");
    }
}