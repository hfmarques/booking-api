using Core.Domain.Dtos.Room;

namespace Core.Domain.Dtos.Hotel;

public class AddHotelDto
{
    public required string Name { get; set; }
    public required List<AddRoomDto> Rooms { get; set; }
}