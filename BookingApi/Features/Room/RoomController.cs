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
    private readonly CheckRoomAvailability checkRoomAvailability;

    public RoomController(ILogger<RoomController> logger,
        GetRooms getRooms,
        GetRoomById getRoomById,
        CheckRoomAvailability checkRoomAvailability)
    {
        this.logger = logger;
        this.getRooms = getRooms;
        this.getRoomById = getRoomById;
        this.checkRoomAvailability = checkRoomAvailability;
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
            return BadRequest(new {ErrorMessage = "Error getting the rooms"});
        }
    }

    [HttpGet("GetById/id/{id:long}")]
    public IActionResult GetById(long id)
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
            return BadRequest(new {ErrorMessage = "Error getting the room"});
        }
    }

    [HttpGet("CheckRoomAvailability/roomId/{roomId:int}/startDate/{startDate:datetime}/endDate/{endDate:datetime}")]
    public IActionResult CheckRoomAvailability(long roomId, DateTime startDate, DateTime endDate)
    {
        try
        {
            var roomAvailable = checkRoomAvailability.Handle(roomId, startDate, endDate);

            return Ok(new {roomAvailable});
        }
        catch (Exception e)
        {
            logger.LogError(e.ToString());
            return BadRequest(new {ErrorMessage = "Error getting the room"});
        }
    }
}