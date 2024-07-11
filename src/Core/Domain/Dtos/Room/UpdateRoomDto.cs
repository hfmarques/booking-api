using Core.Domain.Enums;

namespace Core.Domain.Dtos.Room;

public class UpdateRoomDto
{
    public required long Id { get; set; }
    public required RoomStatusId StatusId { get; set; }
}