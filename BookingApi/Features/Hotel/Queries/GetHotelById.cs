using Data;

namespace BookingApi.Features.Hotel.Queries;

public class GetHotelById
{
    private readonly IUnitOfWork unitOfWork;

    public GetHotelById(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }

    public Model.Hotel? Handle(long id)
    {
        return unitOfWork.Hotels.Get(id);
    }
}