namespace Core.Domain.Entities;

public class Customer : DatabaseEntity
{
    public required string Name { get; set; }
    public required string Phone { get; set; }
    public string? Address { get; set; }
    // public virtual ICollection<Booking> Bookings { get; set; }
}