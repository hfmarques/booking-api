using Core.Domain.Dtos.Room;
using Core.Features.Room.Commands;
using Core.Features.Room.Queries;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Apis.Room.Commands;

public static class PutRoomApi
{
    public static void MapPutRoomApi(
        this IEndpointRouteBuilder routes,
        RouteGroupBuilder group)
    {
        group.MapPut("/{id:long}",
            async (
                [FromServices] IUpdateRoom updateRoom,
                [FromServices] ILogger<IUpdateRoom> logger,
                long id,
                [FromBody] UpdateRoomDto dto) =>
            {
                try
                {
                    if (id != dto.Id)
                        return Results.BadRequest("Room id must be equal");

                    await updateRoom.Handle(dto);

                    return Results.NoContent();
                }
                catch (ArgumentException e)
                {
                    logger.LogWarning("{Exception}", e.ToString());
                    return Results.BadRequest(e.Message);
                }
                catch (Exception e)
                {
                    logger.LogError("{Exception}", e.ToString());
                    return Results.BadRequest("There was an error updating the room");
                }
            });
    }
}
