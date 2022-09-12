using Data;
using Model.Enum;

namespace BookingApi.Features.Booking.Commands;

public class BookRoom
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IVerifyBookingAvailability verifyBookingAvailability;

    public BookRoom(
        IUnitOfWork unitOfWork,
        IVerifyBookingAvailability verifyBookingAvailability)
    {
        this.unitOfWork = unitOfWork;
        this.verifyBookingAvailability = verifyBookingAvailability;
    }

    public void Handle(Model.Booking booking)
    {
        var existedBooking =
            unitOfWork.Bookings.Find(x => x.RoomId == booking.RoomId).ToList();

        if (verifyBookingAvailability.Handle(booking, existedBooking))
        {
            booking.Status = BookingStatus.Confirmed;
            booking.StartDate = booking.StartDate.Date;
            booking.EndDate = booking.EndDate.Date;
            unitOfWork.Bookings.Add(booking);
            unitOfWork.SaveChanges();
        }
        else
        {
            throw new BookingException("A reservation already exists on the requested dates.");
        }
    }
}