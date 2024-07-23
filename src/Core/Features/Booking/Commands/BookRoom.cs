using System.Collections.Immutable;
using Core.Domain.Dtos.Booking;
using Core.Domain.Entities;
using Core.Domain.Enums;
using Core.Repositories;
using Microsoft.Extensions.Logging;
using Serilog.Context;

namespace Core.Features.Booking.Commands;

public interface IBookRoom
{
    Task<Domain.Entities.Booking> Handle(BookRoomDto dto);
}
public class BookRoom(
    IBookingQueryRepository queryRepository,
    ICommandRepository<Domain.Entities.Booking> commandRepository,
    IVerifyBookingAvailability verifyBookingAvailability,
    ILogger<BookRoom> logger
    ) : IBookRoom
{
    public async Task<Domain.Entities.Booking> Handle(BookRoomDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);
        
        var correlationId = Guid.NewGuid();
        using var _ = LogContext.PushProperty("CorrelationId", correlationId);

        logger.LogInformation("Received a new book. Booking: {Booking}", dto);  
        
        var booking = new Domain.Entities.Booking
        {
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            RoomId = dto.RoomId,
            CustomerId = dto.CustomerId
        };

        logger.LogInformation("Retrieving existing booking for this room");
        var existedBookings =
            await queryRepository.FindAsync(x => x.RoomId == booking.RoomId);

        logger.LogInformation("Verifying book availability");
        if (!await verifyBookingAvailability.Handle(booking, existedBookings.ToImmutableList()))
        {
            logger.LogWarning("Booking date not available");
            throw new ArgumentException("Booking date not available");
        }
        
        logger.LogInformation("Booking date available, saving into database");
        
        booking.StatusId = BookingStatusId.Confirmed;
        booking.CorrelationId = correlationId;
        
        await commandRepository.AddAsync(booking);
        
        logger.LogInformation("Booking saved");

        return booking;
    }
}