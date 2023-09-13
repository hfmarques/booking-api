using Core.Repositories;

namespace Core.Features.Hotel.Queries;

public interface IGetHotelWithRoomsById
{
    Task<Core.Domain.Entities.Hotel?> Handle(long id);
}
public class GetHotelWithRoomsById : IGetHotelWithRoomsById
{
    private readonly IHotelQueryRepository _hotelQueryRepository;

    public GetHotelWithRoomsById(IHotelQueryRepository hotelQueryRepository)
    {
        _hotelQueryRepository = hotelQueryRepository;
    }

    public async Task<Core.Domain.Entities.Hotel?> Handle(long id) => 
        await _hotelQueryRepository.GetHotelWithRoomsById(id);
}