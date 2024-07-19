using System.Collections.Immutable;
using Core.Domain.Dtos.Booking;
using Core.Repositories;
using Microsoft.Extensions.Logging;
using Serilog.Context;

namespace Core.Features.Booking.Commands;

public interface IUpdateBooking
{
    Task<Domain.Entities.Booking?> Handle(UpdateBookingDto dto);
}
public class UpdateBooking(
    IBookingQueryRepository queryRepository,
    ICommandRepository<Domain.Entities.Booking> commandRepository,
    IVerifyBookingAvailability verifyBookingAvailability,
    ILogger<UpdateBooking> logger) 
    : IUpdateBooking
{

    public async Task<Domain.Entities.Booking?> Handle(UpdateBookingDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(dto.Id);

        var booking = await queryRepository.GetBookingById(dto.Id);
        
        if (booking is null)
        {
            logger.LogWarning("Booking {Id} does not exists", dto.Id);
            throw new ArgumentException($"Booking {dto.Id} does not exists");
        }
        
        using var _ = LogContext.PushProperty("CorrelationId", booking.CorrelationId);
        logger.LogInformation("Received booking update. Booking: {Booking}", dto);

        logger.LogInformation("Retrieving existing bookings");
        var existedBookings =
            await queryRepository.FindAsync(x =>
                x.RoomId == dto.RoomId &&
                x.Id != booking.Id);
        
        logger.LogInformation("Updating book data");
        booking.StartDate = dto.StartDate;
        booking.EndDate = dto.EndDate;
        booking.RoomId = dto.RoomId;
        booking.CustomerId = dto.CustomerId;
        
        if (!await verifyBookingAvailability.Handle(booking, existedBookings.ToImmutableList()))
        {
            logger.LogWarning("Booking date not available");
            return null;
        }

        logger.LogInformation("Booking is valid, updating database");
        await commandRepository.UpdateAsync();
        return booking;
    }
}