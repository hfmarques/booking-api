namespace Core.Domain.Dtos.Hotel;

public class AddHotelDto
{
    public required string Name { get; set; }
    public required List<AddHotelRoomDto> Rooms { get; set; }
}