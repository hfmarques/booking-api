using BookingApi.Features.Booking.Queries;
using Microsoft.AspNetCore.Mvc;

namespace BookingApi.Features.Booking;

[ApiController]
[Route("[controller]")]
public class BookingController : ControllerBase
{
    private readonly ILogger<BookingController> logger;
    private readonly GetBookings getBookings;
    private readonly GetBookingById getBookingById;

    public BookingController(ILogger<BookingController> logger, GetBookings getBookings, GetBookingById getBookingById)
    {
        this.logger = logger;
        this.getBookings = getBookings;
        this.getBookingById = getBookingById;
    }

    [HttpGet("GetAll")]
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

    [HttpGet("GetById/id/{id:long}")]
    public IActionResult GetById(long id)
    {
        try
        {
            var booking = getBookingById.Handle(id);

            if (booking is null)
                return NotFound();

            return Ok(booking);
        }
        catch (Exception e)
        {
            logger.LogError(e.ToString());
            return BadRequest("Error getting the booking");
        }
    }
}