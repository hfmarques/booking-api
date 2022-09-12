using Data;

namespace BookingApi.Features.Booking.Commands;

public class UpdateBooking
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IVerifyBookingAvailability verifyBookingAvailability;

    public UpdateBooking(IUnitOfWork unitOfWork, IVerifyBookingAvailability verifyBookingAvailability)
    {
        this.unitOfWork = unitOfWork;
        this.verifyBookingAvailability = verifyBookingAvailability;
    }

    public void Handle(Model.Booking booking)
    {
        if (booking is null)
            throw new ArgumentNullException(nameof(booking));

        if (booking.Id == 0)
            throw new ArgumentException("Id cannot be Zero");

        var unmodifiedBooking = unitOfWork.Bookings.Get(booking.Id);

        if (unmodifiedBooking is null)
        {
            throw new ArgumentException("Booking does not exists");
        }

        var existedBookings =
            unitOfWork.Bookings.Find(x =>
                    x.RoomId == unmodifiedBooking.RoomId &&
                    x.Id != unmodifiedBooking.Id)
                .ToList();

        if (verifyBookingAvailability.Handle(booking, existedBookings))
        {
            unmodifiedBooking.StartDate = booking.StartDate.Date;
            unmodifiedBooking.EndDate = booking.EndDate.Date;
            unmodifiedBooking.Status = booking.Status;
            unitOfWork.SaveChanges();
        }
        else
        {
            throw new BookingException("A reservation already exists on the requested dates.");
        }
    }
}