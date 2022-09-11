using Microsoft.EntityFrameworkCore;
using Model;

namespace Data.Repository;

public class RoomRepository : Repository<Room>, IRoomRepository
{
    public RoomRepository(DbContext context) : base(context)
    {
    }
}