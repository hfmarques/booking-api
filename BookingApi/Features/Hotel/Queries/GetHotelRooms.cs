using Data;

namespace BookingApi.Features.Hotel.Queries;

public class GetHotelRooms
{
    private readonly IUnitOfWork unitOfWork;

    public GetHotelRooms(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }

    public List<Model.Hotel> Handle()
    {
        return unitOfWork.Hotels.GetHotelRooms().ToList();
    }
}