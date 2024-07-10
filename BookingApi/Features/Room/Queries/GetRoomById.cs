using Data;

namespace BookingApi.Features.Room.Queries;

public class GetRoomById
{
    private readonly IUnitOfWork unitOfWork;

    public GetRoomById(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }

    public Model.Room? Handle(long id)
    {
        return unitOfWork.Rooms.Get(id);
    }
}