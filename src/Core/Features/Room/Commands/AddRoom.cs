using Core.Domain.Dtos.Room;
using Core.Domain.Enums;
using Core.Features.Hotel.Queries;
using Core.Repositories;
using Microsoft.Extensions.Logging;
using Serilog.Context;

namespace Core.Features.Room.Commands;

public interface IAddRoom
{
    Task<Domain.Entities.Room> Handle(AddRoomDto dto);
}
public class AddRoom(
    IGetHotelById getHotelById,
    ICommandRepository<Domain.Entities.Room> commandRepository,
    ILogger<AddRoom> logger) : IAddRoom
{
    public async Task<Domain.Entities.Room> Handle(AddRoomDto dto)
    {
        var correlationId = Guid.NewGuid();
        using var _ = LogContext.PushProperty("CorrelationId", correlationId);

        logger.LogInformation("Received a new room to be saved. Room: {Room}", dto);
        
        ArgumentNullException.ThrowIfNull(dto);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(dto.Number);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(dto.HotelId);

        var hotel = await getHotelById.Handle(dto.HotelId);
        ArgumentNullException.ThrowIfNull(hotel);
        
        if(hotel.Rooms.Exists(x => x.Number == dto.Number))
            throw new ArgumentException("This hotel already has a room with this number");
        
        logger.LogInformation("Room data is fine, inserting into database");

        var room = new Domain.Entities.Room
        {
            Number = dto.Number,
            HotelId = dto.HotelId,
            StatusId = dto.StatusId ?? RoomStatusId.Available,
            CorrelationId = correlationId
        };

        await commandRepository.AddAsync(room);
        
        logger.LogInformation("Room inserted into database");

        return room;
    }
}