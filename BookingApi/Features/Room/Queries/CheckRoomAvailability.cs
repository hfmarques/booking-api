using BookingApi.Features.Booking.Commands;
using Data;

namespace BookingApi.Features.Room.Queries;

public class CheckRoomAvailability
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IVerifyBookingOverlapping verifyBookingOverlapping;

    public CheckRoomAvailability(IUnitOfWork unitOfWork, IVerifyBookingOverlapping verifyBookingOverlapping)
    {
        this.unitOfWork = unitOfWork;
        this.verifyBookingOverlapping = verifyBookingOverlapping;
    }

    public bool Handle(long roomId, DateTime startDate, DateTime endDate)
    {
        var bookings = unitOfWork.Bookings.Find(x => x.RoomId == roomId).ToList();
        return verifyBookingOverlapping.Handle(startDate, endDate, roomId, bookings);
    }
}