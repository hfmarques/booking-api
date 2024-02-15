using Core.Domain.Dtos.Hotel;
using Core.Repositories;
using System.Linq;

namespace Core.Features.Hotel.Commands;

public interface IAddHotel
{
    Task<Domain.Entities.Hotel> Handle(AddHotelDto dto);
}
public class AddHotel(ICommandRepository<Domain.Entities.Hotel> commandRepository) : IAddHotel
{
    public async Task<Domain.Entities.Hotel> Handle(AddHotelDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);
        ArgumentException.ThrowIfNullOrWhiteSpace(dto.Name);
        ArgumentNullException.ThrowIfNull(dto.Rooms);
        if (dto.Rooms.Count == 0)
            throw new ArgumentException("The hotel needs at least one room");

        var roomNumbers = dto.Rooms.Select(r => r.Number).ToList();
        if(roomNumbers.Count != roomNumbers.Distinct().Count())
            throw new ArgumentException("Room numbers should not repeat");
        
        var hotel = new Domain.Entities.Hotel
        {
            Name = dto.Name,
            Rooms = dto.Rooms.Select(r => new Domain.Entities.Room {Number = r.Number}).ToList()
        };
        
        await commandRepository.AddAsync(hotel);

        return hotel;
    }
}