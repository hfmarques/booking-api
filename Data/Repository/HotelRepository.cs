using Microsoft.EntityFrameworkCore;
using Model;

namespace Data.Repository;

public class HotelRepository : Repository<Hotel>, IHotelRepository
{
    public HotelRepository(DbContext context) : base(context)
    {
    }
}