using Core.Domain.Entities;
using Core.Domain.Enums;

namespace Core.Features.Booking.Commands;

public interface IVerifyBookingOverlapping
{
    bool Handle(DateOnly startDate, DateOnly endDate, long roomId, IReadOnlyCollection<Domain.Entities.Booking> bookings);
}

public class VerifyBookingOverlapping : IVerifyBookingOverlapping
{
    public bool Handle(DateOnly startDate, DateOnly endDate, long roomId, IReadOnlyCollection<Domain.Entities.Booking> bookings)
    {
        var overlappingBooking =
            bookings.FirstOrDefault(x =>
                startDate <= x.EndDate &&
                x.StartDate <= endDate &&
                x.StatusId is BookingStatusId.Confirmed &&
                x.RoomId == roomId);

        return overlappingBooking is null;
    }
}