using Core.Domain.Enums;

namespace Core.Domain.Dtos.Hotel;

public class AddHotelRoomDto
{
    public required int Number { get; set; }
    public RoomStatusId? StatusId { get; set; }
}