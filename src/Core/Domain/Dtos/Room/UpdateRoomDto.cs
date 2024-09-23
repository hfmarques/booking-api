using Core.Domain.Enums;

namespace Core.Domain.Dtos.Room;

public record UpdateRoomDto(long Id, RoomStatusId StatusId);
