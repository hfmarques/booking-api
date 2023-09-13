using Core.Repositories;

namespace Core.Features.Hotel.Queries;

public interface IGetHotels
{
    Task<IEnumerable<Domain.Entities.Hotel>> Handle();
}
public class GetHotels : IGetHotels
{
    private readonly IHotelQueryRepository _hotelQueryRepository;

    public GetHotels(IHotelQueryRepository hotelQueryRepository)
    {
        _hotelQueryRepository = hotelQueryRepository;
    }

    public async Task<IEnumerable<Domain.Entities.Hotel>> Handle() => 
        await _hotelQueryRepository.GetAllAsync();
}