using Core.Repositories;

namespace Core.Features.Room.Queries;

public interface IGetRooms
{
    Task<List<Domain.Entities.Room>> Handle();
}

public class GetRooms(IQueryRepository<Domain.Entities.Room> queryRepository) : IGetRooms
{
    public async Task<List<Domain.Entities.Room>> Handle() => (await queryRepository.GetAllAsync()).ToList();
}