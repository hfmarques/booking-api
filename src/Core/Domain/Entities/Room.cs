namespace Core.Domain.Entities;

public class Room : DatabaseEntity
{
    public required int Number { get; set; }
    public long HotelId { get; set; }
    // public virtual ICollection<Booking> Bookings { get; set; }
}