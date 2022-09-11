using Data;

namespace BookingApi.Features.Booking.Queries;

public class GetBookings
{
    private readonly IUnitOfWork unitOfWork;

    public GetBookings(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }

    public List<Model.Booking> Handle()
    {
        return unitOfWork.Bookings.GetAll().ToList();
    }
}