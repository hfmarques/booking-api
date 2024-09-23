using Core.Domain.Enums;

namespace Core.Domain.Dtos.Hotel;

public record AddHotelRoomDto(int Number, RoomStatusId? StatusId = null);
