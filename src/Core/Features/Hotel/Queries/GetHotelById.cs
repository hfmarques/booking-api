using Core.Repositories;

namespace Core.Features.Hotel.Queries;

public interface IGetHotelById
{
    Task<Domain.Entities.Hotel?> Handle(long id);
}
public class GetHotelById(IHotelQueryRepository hotelQueryRepository) : IGetHotelById
{
    public async Task<Domain.Entities.Hotel?> Handle(long id) => 
        await hotelQueryRepository.GetHotelById(id);
}