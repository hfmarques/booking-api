using Microsoft.EntityFrameworkCore;
using Model;

namespace Data.Repository;

public class HotelRepository : Repository<Hotel>, IHotelRepository
{
    private EfContext context => (Context as EfContext)!;

    public HotelRepository(DbContext context) : base(context)
    {
    }

    public IEnumerable<Hotel> GetHotelRooms()
    {
        var hotels = context.Hotels
            .Include(x => x.Room)
            .ToList();

        return hotels;
    }
}