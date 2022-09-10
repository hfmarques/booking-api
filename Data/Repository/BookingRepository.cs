using Microsoft.EntityFrameworkCore;
using Model;

namespace Data.Repository;

public class BookingRepository : Repository<Booking>, IBookingRepository
{
    private EfContext context => (Context as EfContext)!;

    public BookingRepository(DbContext context) : base(context)
    {
    }
}