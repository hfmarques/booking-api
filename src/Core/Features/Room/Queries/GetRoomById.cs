using Core.Repositories;

namespace Core.Features.Room.Queries;

public interface IGetRoomById
{
    Task<Domain.Entities.Room?> Handle(long id);
}
public class GetRoomById : IGetRoomById
{
    private IQueryRepository<Domain.Entities.Room> _queryRepository;

    public GetRoomById(IQueryRepository<Domain.Entities.Room> queryRepository)
    {
        _queryRepository = queryRepository;
    }

    public async Task<Domain.Entities.Room?> Handle(long id) => await _queryRepository.GetByIdAsync(id);
}