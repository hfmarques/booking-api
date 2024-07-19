using Core.Domain.Entities;
using Core.Domain.Enums;
using Core.Repositories;
using Microsoft.Extensions.Logging;
using Serilog.Context;

namespace Core.Features.Booking.Commands;

public interface ICancelBooking
{
    Task<Domain.Entities.Booking> Handle(long id);
}
public class CancelBooking(
    IBookingQueryRepository queryRepository,
    ICommandRepository<Domain.Entities.Booking> commandRepository,
    ILogger<CancelBooking> logger)
    : ICancelBooking
{
    public async Task<Domain.Entities.Booking> Handle(long id)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id);

        var booking = await queryRepository.GetBookingById(id);
        if (booking is null)
        {
            logger.LogWarning("Booking {Id} does not exists", id);
            throw new ArgumentException($"Booking {id} does not exists");
        }

        using var _ = LogContext.PushProperty("CorrelationId", booking.CorrelationId);
        logger.LogInformation("Received booking cancellation request. Booking: {Booking}", booking);
        
        booking.StatusId = BookingStatusId.Cancelled;

        await commandRepository.UpdateAsync();
        
        logger.LogInformation("Booking cancelled");
        
        return booking;
    }
}