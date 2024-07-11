using Core.Domain.Entities;

namespace Core.Repositories
{
    public interface IHotelQueryRepository : IQueryRepository<Hotel>
    {
        Task<Hotel?> GetHotelById(long id);
    }
}