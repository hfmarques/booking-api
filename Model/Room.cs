namespace Model;

#nullable disable
public class Room
{
    public int Id { get; set; }
    public int Number { get; set; }
    public virtual ICollection<Booking> Bookings { get; set; }
}