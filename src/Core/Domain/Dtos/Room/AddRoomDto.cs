using Core.Domain.Enums;

namespace Core.Domain.Dtos.Room;

public class AddRoomDto
{
    public required int Number { get; set; }
    public required long HotelId { get; set; }
    public RoomStatusId? StatusId { get; set; }
}