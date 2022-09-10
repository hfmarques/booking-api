using BookingApi.Features.Booking.Queries;
using Microsoft.AspNetCore.Mvc;

namespace BookingApi.Features.Booking;

[ApiController]
[Route("[controller]")]
public class BookingController : ControllerBase
{
    private readonly ILogger<BookingController> logger;
    private readonly GetBookings getBookings;

    public BookingController(ILogger<BookingController> logger, GetBookings getBookings)
    {
        this.logger = logger;
        this.getBookings = getBookings;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        try
        {
            var bookings = getBookings.Handle();

            if (bookings.Count == 0)
                return NotFound();

            return Ok(bookings);
        }
        catch (Exception e)
        {
            logger.LogError(e.ToString());
            return BadRequest("Error getting the bookings");
        }
    }
}