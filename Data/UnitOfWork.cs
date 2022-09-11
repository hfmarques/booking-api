using Data.Repository;
using Microsoft.EntityFrameworkCore;

namespace Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly DbContext context;

    public UnitOfWork(DbContext context)
    {
        this.context = context;
        Hotels = new HotelRepository(context);
        Rooms = new RoomRepository(context);
        Bookings = new BookingRepository(context);
        Customers = new CustomerRepository(context);
    }

    public void Dispose()
    {
        context.Dispose();
    }

    public IHotelRepository Hotels { get; }
    public IRoomRepository Rooms { get; }
    public IBookingRepository Bookings { get; }
    public ICustomerRepository Customers { get; }

    public int SaveChanges()
    {
        return context.SaveChanges();
    }
}