using Data;

namespace BookingApi.Features.Hotel.Queries;

public class GetHotels
{
    private readonly IUnitOfWork unitOfWork;

    public GetHotels(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }

    public List<Model.Hotel> Handle()
    {
        return unitOfWork.Hotels.GetAll().ToList();
    }
}