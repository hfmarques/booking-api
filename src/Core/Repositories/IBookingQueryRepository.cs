using Core.Domain.Entities;

namespace Core.Repositories;

public interface IBookingQueryRepository : IQueryRepository<Booking>
{
    Task<Booking?> GetBookingById(long id);
    Task<List<Booking>> GetActiveBookingsByRoomId(long id);
    Task<List<Booking>> GetUpcomingBookings();
}