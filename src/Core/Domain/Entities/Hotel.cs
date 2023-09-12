namespace Core.Domain.Entities;

public class Hotel : DatabaseEntity
{
    public required string Name { get; set; }
    public virtual List<Room> Room { get; set; } = new();
}