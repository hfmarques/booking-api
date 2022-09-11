using Data;

namespace BookingApi.Features.Booking.Commands;

public class UpdateBook
{
    private readonly IUnitOfWork unitOfWork;

    public UpdateBook(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }

    public void Handle(Model.Booking booking)
    {
        if (booking is null)
            throw new ArgumentNullException(nameof(booking));

        if (booking.Id == 0)
            throw new ArgumentException("Id cannot be Zero");

        var existingBooking = unitOfWork.Bookings.Get(booking.Id);

        if (existingBooking is null)
        {
            throw new ArgumentException("Booking does not exists");
        }

        existingBooking.StartDate = booking.StartDate;
        existingBooking.EndDate = booking.EndDate;
        existingBooking.Status = booking.Status;

        unitOfWork.SaveChanges();
    }
}