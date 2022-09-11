namespace Model;

#nullable disable
public class Hotel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<Room> Room { get; set; }
}