using Core.Domain.Entities;
using Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class HotelQueryRepository : QueryRepository<Hotel>, IHotelQueryRepository
{
    private readonly PostgresDbContext _context;
    public HotelQueryRepository(DbContext context) : base(context)
    {
        _context = (PostgresDbContext)context;
    }
    public async Task<Hotel?> GetHotelWithRoomsById(long id)
    {
        return await _context.Set<Hotel>()
            .Include(x => x.Rooms)
            .Where(x => x.Id == id)
            .SingleOrDefaultAsync();
    }
}