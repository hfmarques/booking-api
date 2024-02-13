using Core.Repositories;

namespace Core.Features.Room.Queries;

public interface IGetRoomById
{
    Task<Domain.Entities.Room?> Handle(long id);
}
public class GetRoomById(IQueryRepository<Domain.Entities.Room> queryRepository) : IGetRoomById
{
    public async Task<Domain.Entities.Room?> Handle(long id) => await queryRepository.GetByIdAsync(id);
}