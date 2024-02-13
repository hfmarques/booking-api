using Core.Repositories;

namespace Core.Features.Room.Queries;

public interface IGetRooms
{
    Task<IEnumerable<Domain.Entities.Room>> Handle();
}

public class GetRooms(IQueryRepository<Domain.Entities.Room> queryRepository) : IGetRooms
{
    public async Task<IEnumerable<Domain.Entities.Room>> Handle() => await queryRepository.GetAllAsync();
}