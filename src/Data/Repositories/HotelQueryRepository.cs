using Core.Domain.Entities;
using Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class HotelQueryRepository(DbContext context) : QueryRepository<Hotel>(context), IHotelQueryRepository
{
    public async Task<Hotel?> GetHotelWithRoomsById(long id)
    {
        return await context.Set<Hotel>()
            .Include(x => x.Rooms)
            .Where(x => x.Id == id)
            .SingleOrDefaultAsync();
    }
}