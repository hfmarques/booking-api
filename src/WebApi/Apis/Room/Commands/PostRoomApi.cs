using Core.Domain.Dtos.Room;
using Core.Features.Room.Commands;
using Core.Features.Room.Queries;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Apis.Room.Commands;

public static class PostRoomApi
{
    public static void MapPostRoomApi(
        this IEndpointRouteBuilder routes,
        RouteGroupBuilder group)
    {
        group.MapPost("/",
            async (
                [FromServices] IAddRoom addRoom,
                [FromServices] ILogger<IAddRoom> logger,
                [FromBody] AddRoomDto dto) =>
            {
                try
                {
                    var room = await addRoom.Handle(dto);

                    return Results.Created($"rooms/{room.Id}", room);
                }
                catch (ArgumentException e)
                {
                    logger.LogWarning("{Exception}", e.ToString());
                    return Results.BadRequest(e.Message);
                }
                catch (Exception e)
                {
                    logger.LogError("{Exception}", e.ToString());
                    return Results.BadRequest("There was an error adding the room");
                }
            });
    }
}
