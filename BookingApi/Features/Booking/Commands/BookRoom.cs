using Data;

namespace BookingApi.Features.Booking.Commands;

public class BookRoom
{
    private readonly IUnitOfWork unitOfWork;

    public BookRoom(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }

    public void Handle(Model.Booking booking)
    {
        if (booking is null)
            throw new ArgumentNullException(nameof(booking));

        var room = unitOfWork.Rooms.Get(booking.RoomId);
        if (room is null)
            throw new ArgumentException("Room does not existed");

        var customer = unitOfWork.Customers.Get(booking.CustomerId);
        if (customer is null)
            throw new ArgumentException("Customer does not existed");

        if ((booking.EndDate - booking.StartDate).TotalDays > 3)
            throw new BookingException("The stay can't be longer than 3 days");

        if ((DateTime.Now - booking.StartDate).TotalDays > 30)
            throw new BookingException("The booking can't be reserved more than 30 days in advance.");
    }
}