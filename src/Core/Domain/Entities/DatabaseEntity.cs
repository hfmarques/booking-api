namespace Core.Domain.Entities;

public abstract class DatabaseEntity
{
    public long Id { get; set; } = 0;
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; } = false;
    public DateTime? DeletedAt { get; set; }
    public Guid CorrelationId { get; set; }
}