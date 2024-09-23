namespace Core.Domain.Dtos.Hotel;

public record AddHotelDto(string Name, List<AddHotelRoomDto> Rooms);
