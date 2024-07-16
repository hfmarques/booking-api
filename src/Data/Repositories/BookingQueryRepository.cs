using Core.Domain.Entities;
using Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class BookingQueryRepository(DbContext context) : QueryRepository<Booking>(context), IBookingQueryRepository
{
    public async Task<Booking?> GetBookingById(long id)
    {
        return await context.Set<Booking>()
            .Include(x => x.Status)
            .Include(x => x.Customer)
            .Include(x => x.Room)
            .Where(x => x.Id == id)
            .SingleOrDefaultAsync();
    }
}