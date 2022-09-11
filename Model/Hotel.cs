namespace Model;

#nullable disable
public class Hotel
{
    public long Id { get; set; }
    public string Name { get; set; }
    public virtual List<Room> Room { get; set; }
}