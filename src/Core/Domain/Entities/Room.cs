using Core.Domain.Enums;

namespace Core.Domain.Entities;

public class Room : DatabaseEntity
{
    public required int Number { get; set; }
    public long HotelId { get; set; }
    public RoomStatusId StatusId { get; set; } = RoomStatusId.Available;
    public RoomStatus? Status { get; set; }
}