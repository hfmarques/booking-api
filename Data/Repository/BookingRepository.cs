using Microsoft.EntityFrameworkCore;
using Model;
using Model.Enum;

namespace Data.Repository;

public class BookingRepository : Repository<Booking>, IBookingRepository
{
    private EfContext context => (Context as EfContext)!;

    public BookingRepository(DbContext context) : base(context)
    {
    }

    public IEnumerable<Booking> GetActiveBookingsByRoomId(long id)
    {
        return context.Bookings.Where(x =>
                x.RoomId == id &&
                x.Status == BookingStatus.Confirmed)
            .ToList();
    }

    public IEnumerable<Booking> GetFutureBookings()
    {
        return context.Bookings.Where(x => x.StartDate.Date >= DateTime.Now.Date)
            .ToList();
    }
}