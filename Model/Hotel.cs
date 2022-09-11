namespace Model;

#nullable disable
public class Hotel
{
    public long Id { get; set; }
    public string Name { get; set; }
    public List<Room> Room { get; set; }
}