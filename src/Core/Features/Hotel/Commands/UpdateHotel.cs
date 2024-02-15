using Core.Domain.Dtos.Hotel;
using Core.Features.Hotel.Queries;
using Core.Repositories;

namespace Core.Features.Hotel.Commands;

public interface IUpdateHotel
{
    Task Handle(UpdateHotelDto dto);
}
public class UpdateHotel(
        ICommandRepository<Domain.Entities.Hotel> commandRepository,
        IGetHotelById getHotelById
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

        hotel.Name = dto.Name;

        await commandRepository.UpdateAsync(hotel);
    }
}