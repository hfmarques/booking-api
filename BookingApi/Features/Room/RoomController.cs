using BookingApi.Features.Room.Queries;
using Microsoft.AspNetCore.Mvc;

namespace BookingApi.Features.Room;

[ApiController]
[Route("[controller]")]
public class RoomController : ControllerBase
{
    private readonly ILogger<RoomController> logger;
    private readonly GetRooms getRooms;

    public RoomController(ILogger<RoomController> logger, GetRooms getRooms)
    {
        this.logger = logger;
        this.getRooms = getRooms;
    }

    [HttpGet]
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
}