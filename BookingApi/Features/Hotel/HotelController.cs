using BookingApi.Features.Hotel.Queries;
using Microsoft.AspNetCore.Mvc;

namespace BookingApi.Features.Hotel;

[ApiController]
[Route("[controller]")]
public class HotelController : ControllerBase
{
    private readonly ILogger<HotelController> logger;
    private readonly GetHotels getHotels;
    private readonly GetHotelRooms getHotelRooms;
    private readonly GetHotelById getHotelById;

    public HotelController(
        ILogger<HotelController> logger,
        GetHotels getHotels,
        GetHotelRooms getHotelRooms,
        GetHotelById getHotelById)
    {
        this.logger = logger;
        this.getHotels = getHotels;
        this.getHotelRooms = getHotelRooms;
        this.getHotelById = getHotelById;
    }

    [HttpGet("GetAll")]
    public IActionResult GetAll()
    {
        try
        {
            var hotels = getHotels.Handle();

            if (hotels.Count == 0)
                return NotFound();

            return Ok(hotels);
        }
        catch (Exception e)
        {
            logger.LogError(e.ToString());
            return BadRequest("Error getting the hotels");
        }
    }

    [HttpGet("GetHotelRooms")]
    public IActionResult GetHotelRooms()
    {
        try
        {
            var hotels = getHotelRooms.Handle();

            if (hotels.Count == 0)
                return NotFound();

            return Ok(hotels);
        }
        catch (Exception e)
        {
            logger.LogError(e.ToString());
            return BadRequest("Error getting the hotels");
        }
    }

    [HttpGet("GetById/id/{id:long}")]
    public IActionResult GerById(long id)
    {
        try
        {
            var hotel = getHotelById.Handle(id);

            if (hotel == null)
                return NotFound();

            return Ok(hotel);
        }
        catch (Exception e)
        {
            logger.LogError(e.ToString());
            return BadRequest("Error getting the hotel");
        }
    }
}