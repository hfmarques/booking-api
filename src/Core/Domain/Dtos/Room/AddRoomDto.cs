using Core.Domain.Enums;

namespace Core.Domain.Dtos.Room;

public record AddRoomDto(int Number, long HotelId, RoomStatusId? StatusId = null);
