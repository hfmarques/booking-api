using Core.Domain.Dtos.Room;
using Core.Features.Hotel.Commands;
using Core.Features.Room.Queries;
using Core.Repositories;
using Microsoft.Extensions.Logging;
using Serilog.Context;

namespace Core.Features.Room.Commands;

public interface IUpdateRoom
{
    Task<Domain.Entities.Room> Handle(UpdateRoomDto dto);
}
public class UpdateRoom(
    IGetRoomById getRoomById,
    ICommandRepository<Domain.Entities.Room> commandRepository,
    ILogger<UpdateRoom> logger) : IUpdateRoom
{
    public async Task<Domain.Entities.Room> Handle(UpdateRoomDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        var room = await getRoomById.Handle(dto.Id);
        
        ArgumentNullException.ThrowIfNull(room);
        
        using var _ = LogContext.PushProperty("CorrelationId", room.CorrelationId);
        
        logger.LogInformation("Received a room to be updated: Room: {Hotel}", dto);

        room.StatusId = dto.StatusId;
        
        await commandRepository.UpdateAsync();

        logger.LogInformation("Room updated");
        
        return room;
    }
}