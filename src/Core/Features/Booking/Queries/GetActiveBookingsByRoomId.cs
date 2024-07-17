using Core.Repositories;

namespace Core.Features.Booking.Queries;

public interface IGetActiveBookingsByRoomId
{
    Task<List<Domain.Entities.Booking>> Handle(long id);
}
public class GetActiveBookingsByRoomId(IBookingQueryRepository queryRepository) : IGetActiveBookingsByRoomId
{
    public async Task<List<Domain.Entities.Booking>> Handle(long id) => 
        await queryRepository.GetActiveBookingsByRoomId(id);
}