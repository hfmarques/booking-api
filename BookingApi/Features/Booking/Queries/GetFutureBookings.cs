using Data;

namespace BookingApi.Features.Booking.Queries;

public class GetFutureBookings
{
    private readonly IUnitOfWork unitOfWork;

    public GetFutureBookings(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }

    public List<Model.Booking> Handle()
    {
        return unitOfWork.Bookings.GetFutureBookings().ToList();
    }
}