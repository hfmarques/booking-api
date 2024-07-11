using Core.Domain.Dtos.Hotel;
using Core.Features.Hotel.Queries;
using Core.Repositories;
using Microsoft.Extensions.Logging;
using Serilog.Context;

namespace Core.Features.Hotel.Commands;

public interface IUpdateHotel
{
    Task Handle(UpdateHotelDto dto);
}
public class UpdateHotel(
        ICommandRepository<Domain.Entities.Hotel> commandRepository,
        IGetHotelById getHotelById,
        ILogger<UpdateHotel> logger
    ) : IUpdateHotel
{
    public async Task Handle(UpdateHotelDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(dto.Id);
        ArgumentException.ThrowIfNullOrWhiteSpace(dto.Name);

        var hotel = await getHotelById.Handle(dto.Id);
        if (hotel is null)
            throw new ArgumentException("The hotel doesnt exist");
        
        using var _ = LogContext.PushProperty("CorrelationId", hotel.CorrelationId);

        logger.LogInformation("Received a hotel to be updated: Hotel: {Hotel}", dto);

        hotel.Name = dto.Name;

        await commandRepository.UpdateAsync(hotel);
        
        logger.LogInformation("Hotel updated");
    }
}