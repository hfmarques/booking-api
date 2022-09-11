using BookingApi.Features.Customer.Queries;
using BookingApi.Features.Room.Queries;
using Microsoft.AspNetCore.Mvc;

namespace BookingApi.Features.Room;

[ApiController]
[Route("[controller]")]
public class RoomController : ControllerBase
{
    private readonly ILogger<RoomController> logger;
    private readonly GetRooms getRooms;
    private readonly GetRoomById getRoomById;

    public RoomController(ILogger<RoomController> logger, GetRooms getRooms, GetRoomById getRoomById)
    {
        this.logger = logger;
        this.getRooms = getRooms;
        this.getRoomById = getRoomById;
    }

    [HttpGet("GetAll")]
    public IActionResult GetAll()
    {
        try
        {
            var rooms = getRooms.Handle();

            if (rooms.Count == 0)
                return NotFound();

            return Ok(rooms);
        }
        catch (Exception e)
        {
            logger.LogError(e.ToString());
            return BadRequest("Error getting the rooms");
        }
    }

    [HttpGet("GetById/id/{id:int}")]
    public IActionResult GetById(int id)
    {
        try
        {
            var room = getRoomById.Handle(id);

            if (room is null)
                return NotFound();

            return Ok(room);
        }
        catch (Exception e)
        {
            logger.LogError(e.ToString());
            return BadRequest("Error getting the room");
        }
    }
}