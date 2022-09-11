using Data;

namespace BookingApi.Features.Booking.Queries;

public class GetBookingById
{
    private readonly IUnitOfWork unitOfWork;

    public GetBookingById(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }

    public Model.Booking? Handle(long id)
    {
        return unitOfWork.Bookings.Get(id);
    }
}