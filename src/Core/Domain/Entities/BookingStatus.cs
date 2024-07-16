using Model.Enum;

namespace Core.Domain.Entities;

public sealed class BookingStatus
{
    public required BookingStatusId Id { get; init; }
    public required string Name { get; init; }
    public required string Label { get; init; }
}