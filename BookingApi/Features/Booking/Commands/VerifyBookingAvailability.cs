using Data;

namespace BookingApi.Features.Booking.Commands;

public interface IVerifyBookingAvailability
{
    bool Handle(Model.Booking booking, List<Model.Booking> bookings);
}

public class VerifyBookingAvailability : IVerifyBookingAvailability
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IVerifyBookingOverlapping verifyBookingOverlapping;

    public VerifyBookingAvailability(IUnitOfWork unitOfWork, IVerifyBookingOverlapping verifyBookingOverlapping)
    {
        this.unitOfWork = unitOfWork;
        this.verifyBookingOverlapping = verifyBookingOverlapping;
    }

    public bool Handle(Model.Booking booking, List<Model.Booking> bookings)
    {
        if (booking is null)
            throw new ArgumentNullException(nameof(booking));

        var room = unitOfWork.Rooms.Get(booking.RoomId);
        if (room is null)
            throw new ArgumentException("Room does not existed");

        var customer = unitOfWork.Customers.Get(booking.CustomerId);
        if (customer is null)
            throw new ArgumentException("Customer does not existed");

        if (booking.StartDate.Date > booking.EndDate.Date)
            throw new BookingException("The startDate must be lower than endDate");

        //All reservations start at least the next day of booking,
        if (booking.StartDate.Date < DateTime.Now.Date)
            throw new BookingException("You cannot book a date in the pass");

        //the stay can’t be longer than 3 days
        if ((booking.EndDate - booking.StartDate).TotalDays > 3)
            throw new BookingException("The stay can't be longer than 3 days");

        //can’t be reserved more than 30 days in advance.
        if ((booking.StartDate - DateTime.Now).TotalDays > 30)
            throw new BookingException("The booking can't be reserved more than 30 days in advance.");

        return verifyBookingOverlapping.Handle(booking.StartDate, booking.EndDate, booking.RoomId, bookings);
    }
}