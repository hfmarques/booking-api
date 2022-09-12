using BookingApi.Features.Booking.Commands;
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
    private readonly BookRoom bookRoom;
    private readonly CancelBooking cancelBooking;
    private readonly UpdateBooking updateBooking;
    private readonly GetActiveBookingsByRoomId getActiveBookingsByRoomId;
    private readonly GetFutureBookings getFutureBookings;

    public BookingController(ILogger<BookingController> logger, GetBookings getBookings, GetBookingById getBookingById,
        BookRoom bookRoom, CancelBooking cancelBooking, UpdateBooking updateBooking,
        GetActiveBookingsByRoomId getActiveBookingsByRoomId, GetFutureBookings getFutureBookings)
    {
        this.logger = logger;
        this.getBookings = getBookings;
        this.getBookingById = getBookingById;
        this.bookRoom = bookRoom;
        this.cancelBooking = cancelBooking;
        this.updateBooking = updateBooking;
        this.getActiveBookingsByRoomId = getActiveBookingsByRoomId;
        this.getFutureBookings = getFutureBookings;
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

    [HttpGet("GetActiveBookingsByRoomId/RoomId/{roomId:long}")]
    public IActionResult GetActiveBookingsByRoomId(long roomId)
    {
        try
        {
            var bookings = getActiveBookingsByRoomId.Handle(roomId);

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

    [HttpGet("GetFutureBookings")]
    public IActionResult GetFutureBookings()
    {
        try
        {
            var bookings = getFutureBookings.Handle();

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

    [HttpPost("BookRoom")]
    public IActionResult BookRoom(Model.Booking booking)
    {
        try
        {
            bookRoom.Handle(booking);
        }
        catch (Exception e) when (e is ArgumentNullException or ArgumentException or BookingException)
        {
            logger.LogError(e.ToString());
            return BadRequest(e.Message);
        }
        catch (Exception e)
        {
            logger.LogError(e.ToString());
            return BadRequest("Unexpected error saving the booking");
        }

        return Created("", booking);
    }

    [HttpPut("CancelBooking/id/{id:long}")]
    public IActionResult CancelBooking(long id)
    {
        try
        {
            var booking = cancelBooking.Handle(id);
            return Ok(booking);
        }
        catch (ArgumentException e)
        {
            logger.LogError(e.ToString());
            return BadRequest(e.Message);
        }
        catch (Exception e)
        {
            logger.LogError(e.ToString());
            return BadRequest("Unexpected error updating the booking");
        }
    }

    [HttpPut("UpdateBooking")]
    public IActionResult UpdateBooking(Model.Booking booking)
    {
        try
        {
            updateBooking.Handle(booking);
            return Ok(booking);
        }
        catch (Exception e) when (e is ArgumentNullException or ArgumentException)
        {
            logger.LogError(e.ToString());
            return BadRequest(e.Message);
        }
        catch (Exception e)
        {
            logger.LogError(e.ToString());
            return BadRequest("Unexpected error updating the booking");
        }
    }
}