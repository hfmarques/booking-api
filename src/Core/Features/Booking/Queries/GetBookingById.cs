using Core.Repositories;

namespace Core.Features.Booking.Queries;

public interface IGetBookingById
{
    Task<Domain.Entities.Booking?> Handle(long id);
}
public class GetBookingById(IBookingQueryRepository queryRepository) : IGetBookingById
{
    public async Task<Domain.Entities.Booking?> Handle(long id) => 
        await queryRepository.GetBookingById(id);
}