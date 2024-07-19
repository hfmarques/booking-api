using Core.Domain.Enums;

namespace Core.Domain.Entities;

public class Booking : DatabaseEntity
{
    public required DateOnly StartDate { get; set; }
    public required DateOnly EndDate { get; set; }
    public BookingStatusId StatusId { get; set; } = BookingStatusId.Confirmed;
    public BookingStatus? Status { get; set; }
    public required long RoomId { get; set; }
    public Room? Room { get; set; }
    public required long CustomerId { get; set; }
    public Customer? Customer { get; set; }
}