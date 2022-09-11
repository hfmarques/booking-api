using Data;
using Model.Enum;

namespace BookingApi.Features.Booking.Commands;

public class CancelBook
{
    private readonly IUnitOfWork unitOfWork;

    public CancelBook(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }

    public Model.Booking Handle(long id)
    {
        if (id == 0)
            throw new ArgumentException("Id cannot be Zero");

        var booking = unitOfWork.Bookings.Get(id);

        if (booking is null)
        {
            throw new ArgumentException("Booking does not exists");
        }

        booking.Status = BookingStatus.Cancelled;
        unitOfWork.SaveChanges();

        return booking;
    }
}