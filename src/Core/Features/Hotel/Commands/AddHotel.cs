using Core.Domain.Dtos.Hotel;
using Core.Repositories;
using Microsoft.Extensions.Logging;
using Serilog.Context;

namespace Core.Features.Hotel.Commands;

public interface IAddHotel
{
    Task<Domain.Entities.Hotel> Handle(AddHotelDto dto);
}
public class AddHotel(
    ICommandRepository<Domain.Entities.Hotel> commandRepository,
    ILogger<AddHotel> logger) : IAddHotel
{
    public async Task<Domain.Entities.Hotel> Handle(AddHotelDto dto)
    {
        var correlationId = Guid.NewGuid();
        using var _ = LogContext.PushProperty("CorrelationId", correlationId);

        logger.LogInformation("Received a new hotel to be saved. Hotel: {Hotel}", dto);

        ArgumentNullException.ThrowIfNull(dto);
        ArgumentException.ThrowIfNullOrWhiteSpace(dto.Name);
        ArgumentNullException.ThrowIfNull(dto.Rooms);
        if (dto.Rooms.Count == 0)
            throw new ArgumentException("The hotel needs at least one room");

        var roomNumbers = dto.Rooms.Select(r => r.Number).ToList();
        if (roomNumbers.Count != roomNumbers.Distinct().Count())
            throw new ArgumentException("Room numbers should not repeat");

        logger.LogInformation("Hotel data is fine, inserting into database");

        var hotel = new Domain.Entities.Hotel
        {
            Name = dto.Name,
            Rooms = dto.Rooms.Select(r => new Domain.Entities.Room {Number = r.Number, CorrelationId = correlationId}).ToList(),
            CorrelationId = correlationId
        };

        await commandRepository.AddAsync(hotel);

        logger.LogInformation("Hotel inserted into database");

        return hotel;
    }
}