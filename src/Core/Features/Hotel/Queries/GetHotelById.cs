using Core.Repositories;

namespace Core.Features.Hotel.Queries;

public interface IGetHotelById
{
    Task<Domain.Entities.Hotel?> Handle(long id);
}
public class GetHotelById : IGetHotelById
{
    private readonly IHotelQueryRepository _hotelQueryRepository;

    public GetHotelById(IHotelQueryRepository hotelQueryRepository)
    {
        _hotelQueryRepository = hotelQueryRepository;
    }

    public async Task<Domain.Entities.Hotel?> Handle(long id) => 
        await _hotelQueryRepository.GetByIdAsync(id);
}