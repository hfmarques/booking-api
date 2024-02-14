using Core.Repositories;

namespace Core.Features.Hotel.Queries;

public interface IGetHotels
{
    Task<List<Domain.Entities.Hotel>> Handle();
}
public class GetHotels(IHotelQueryRepository hotelQueryRepository) : IGetHotels
{
    public async Task<List<Domain.Entities.Hotel>> Handle() => 
        (await hotelQueryRepository.GetAllAsync()).ToList();
}