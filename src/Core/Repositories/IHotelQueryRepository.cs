using Core.Domain.Entities;
using System.Linq.Expressions;

namespace Core.Repositories
{
    public interface IHotelQueryRepository : IQueryRepository<Hotel>
    {
        Task<Hotel?> GetHotelWithRoomsById(long id);
    }
}