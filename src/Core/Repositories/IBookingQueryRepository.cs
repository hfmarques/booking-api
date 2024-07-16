using Core.Domain.Entities;

namespace Core.Repositories;

public interface IBookingQueryRepository
{
    Task<Booking?> GetBookingById(long id);
}