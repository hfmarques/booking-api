using Core.Domain.Entities;
using Core.Repositories;
using Microsoft.EntityFrameworkCore;
using Model.Enum;

namespace Data.Repositories;

public class BookingQueryRepository(DbContext context) : QueryRepository<Booking>(context), IBookingQueryRepository
{
    private IQueryable<Booking> MainQuery() =>
        context.Set<Booking>()
            .Include(x => x.Status)
            .Include(x => x.Customer)
            .Include(x => x.Room);
    public async Task<Booking?> GetBookingById(long id) =>
        await MainQuery()
            .Where(x => x.Id == id)
            .SingleOrDefaultAsync();
    public async Task<List<Booking>> GetActiveBookingsByRoomId(long id) =>
        await MainQuery()
            .Where(x => x.RoomId == id && 
                        x.StatusId == BookingStatusId.Confirmed)
            .ToListAsync();
    public async Task<List<Booking>> GetUpcomingBookings() =>
        await MainQuery()
            .Where(x => x.StartDate > DateOnly.FromDateTime(DateTime.Now))
            .ToListAsync();
}