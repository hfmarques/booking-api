using Data.Repository;

namespace Data;

public interface IUnitOfWork : IDisposable
{
    IHotelRepository Hotels { get; }
    IRoomRepository Rooms { get; }
    IBookingRepository Bookings { get; }
    ICustomerRepository Customers { get; }

    int SaveChanges();
}