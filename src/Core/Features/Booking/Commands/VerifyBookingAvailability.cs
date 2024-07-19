using Core.Repositories;
using Microsoft.Extensions.Logging;

namespace Core.Features.Booking.Commands;

public interface IVerifyBookingAvailability
{
    Task<bool> Handle(Domain.Entities.Booking booking, IReadOnlyCollection<Domain.Entities.Booking> bookings);
}

public class VerifyBookingAvailability(
    IQueryRepository<Domain.Entities.Room> roomQueryRepository,
    ICustomerQueryRepository customerQueryRepository,
    IVerifyBookingOverlapping verifyBookingOverlapping,
    ILogger<VerifyBookingAvailability> logger) : 
    IVerifyBookingAvailability
{
    public async Task<bool> Handle(Domain.Entities.Booking booking, IReadOnlyCollection<Domain.Entities.Booking> bookings)
    {
        ArgumentNullException.ThrowIfNull(booking);

        var room = await roomQueryRepository.GetByIdAsync(booking.RoomId);
        ArgumentNullException.ThrowIfNull(room);

        var customer = await customerQueryRepository.GetByIdAsync(booking.CustomerId);
        ArgumentNullException.ThrowIfNull(customer);

        //'DAY' in the hotel room starts from 00:00 to 23:59:59
        if (booking.StartDate > booking.EndDate)
        {
            logger.LogWarning("The startDate must be lower than endDate");
            throw new ArgumentException("The startDate must be lower than endDate");
        }

        //All reservations start at least the next day of booking,
        if (booking.StartDate < DateOnly.FromDateTime(DateTime.Now))
        {
            logger.LogWarning("You cannot book a date in the pass");
            throw new ArgumentException("You cannot book a date in the pass");
        }

        //the stay can’t be longer than 3 days
        if (booking.EndDate.Day - booking.StartDate.Day > 3)
        {
            logger.LogWarning("The stay can't be longer than 3 days");
            throw new ArgumentException("The stay can't be longer than 3 days");
        }

        //can’t be reserved more than 30 days in advance.
        if (booking.StartDate.DayNumber - DateOnly.FromDateTime(DateTime.Now.Date).DayNumber > 30)
        {
            logger.LogWarning("The booking can't be reserved more than 30 days in advance");
            throw new ArgumentException("The booking can't be reserved more than 30 days in advance");
        }

        return verifyBookingOverlapping.Handle(booking.StartDate, booking.EndDate, booking.RoomId, bookings);
    }
}