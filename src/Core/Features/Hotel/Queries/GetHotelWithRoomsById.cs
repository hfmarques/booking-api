using Core.Repositories;

namespace Core.Features.Hotel.Queries;

public interface IGetHotelWithRoomsById
{
    Task<Core.Domain.Entities.Hotel?> Handle(long id);
}
public class GetHotelWithRoomsById(IHotelQueryRepository hotelQueryRepository) : IGetHotelWithRoomsById
{
    public async Task<Core.Domain.Entities.Hotel?> Handle(long id) => 
        await hotelQueryRepository.GetHotelWithRoomsById(id);
}