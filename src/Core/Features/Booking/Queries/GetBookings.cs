using Core.Repositories;

namespace Core.Features.Booking.Queries;

public interface IGetBookings
{
    Task<List<Domain.Entities.Booking>> Handle();
}
public class GetBookings(IBookingQueryRepository queryRepository) : IGetBookings
{
    public async Task<List<Domain.Entities.Booking>> Handle() => 
        (await queryRepository.GetAllAsync()).ToList();
}