namespace Core.Domain.Entities;

public class Hotel : DatabaseEntity
{
    public required string Name { get; set; }
    public List<Room> Rooms { get; set; } = [];
}