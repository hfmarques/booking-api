using Model.Enum;

namespace Core.Domain.Entities;

public class Booking : DatabaseEntity
{
    public required DateTime StartDate { get; set; }
    public required DateTime EndDate { get; set; }
    public BookingStatusId StatusId { get; set; } = BookingStatusId.Confirmed;
    public BookingStatus? Status { get; set; }
    public required long RoomId { get; set; }
    public Room? Room { get; set; }
    public required long CustomerId { get; set; }
    public Customer? Customer { get; set; }
}