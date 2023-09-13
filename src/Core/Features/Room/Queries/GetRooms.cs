using Core.Repositories;

namespace Core.Features.Room.Queries;

public interface IGetRooms
{
    Task<IEnumerable<Domain.Entities.Room>> Handle();
}

public class GetRooms : IGetRooms
{
    private readonly IQueryRepository<Domain.Entities.Room> _queryRepository;

    public GetRooms(IQueryRepository<Domain.Entities.Room> queryRepository)
    {
        _queryRepository = queryRepository;
    }
    public async Task<IEnumerable<Domain.Entities.Room>> Handle() => await _queryRepository.GetAllAsync();
}