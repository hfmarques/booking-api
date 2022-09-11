using Model;

namespace Data.Repository;

public interface IHotelRepository : IRepository<Hotel>
{
    IEnumerable<Hotel> GetHotelRooms();
}