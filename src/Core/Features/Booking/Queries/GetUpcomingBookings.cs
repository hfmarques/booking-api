using Core.Repositories;

namespace Core.Features.Booking.Queries;

public interface IGetUpcomingBookings
{
    Task<List<Domain.Entities.Booking>> Handle();
}
public class GetUpcomingBookings(IBookingQueryRepository queryRepository) : IGetUpcomingBookings
{
    public async Task<List<Domain.Entities.Booking>> Handle() => 
        await queryRepository.GetUpcomingBookings();
}