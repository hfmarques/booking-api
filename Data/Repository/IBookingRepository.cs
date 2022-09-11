using Model;

namespace Data.Repository;

public interface IBookingRepository : IRepository<Booking>
{
    IEnumerable<Booking> GetActiveBookingsByRoomId(long id);
    IEnumerable<Booking> GetFutureBookings();
}