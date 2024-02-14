using WebApi.Apis.Room.Queries;

namespace WebApi.Apis.Room;

public static class RoomApiMap
{
    public static void AddApisFromRooms(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/rooms")
            .WithTags("Room");

        app.MapGetRoomsApi(group);
        app.MapGetRoomByIdApi(group);
    }
}