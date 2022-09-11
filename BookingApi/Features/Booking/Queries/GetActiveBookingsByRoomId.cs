using Data;

namespace BookingApi.Features.Booking.Queries;

public class GetActiveBookingsByRoomId
{
    private readonly IUnitOfWork unitOfWork;

    public GetActiveBookingsByRoomId(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }

    public List<Model.Booking> Handle(long id)
    {
        return unitOfWork.Bookings.GetActiveBookingsByRoomId(id).ToList();
    }
}