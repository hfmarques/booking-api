using Data;

namespace BookingApi.Features.Room.Queries;

public class GetRooms
{
    private readonly IUnitOfWork unitOfWork;

    public GetRooms(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }

    public List<Model.Room> Handle()
    {
        return unitOfWork.Rooms.GetAll().ToList();
    }
}