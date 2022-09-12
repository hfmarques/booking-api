using Model.Enum;

namespace BookingApi.Features.Booking.Commands;

public interface IVerifyBookingOverlapping
{
    bool Handle(DateTime startDate, DateTime endDate, long roomId, List<Model.Booking> bookings);
}

public class VerifyBookingOverlapping : IVerifyBookingOverlapping
{
    public bool Handle(DateTime startDate, DateTime endDate, long roomId, List<Model.Booking> bookings)
    {
        var overlappingBooking =
            bookings.FirstOrDefault(x =>
                startDate.Date <= x.EndDate.Date &&
                x.StartDate.Date <= endDate.Date &&
                x.Status == BookingStatus.Confirmed &&
                x.RoomId == roomId);

        return overlappingBooking is null;
    }
}