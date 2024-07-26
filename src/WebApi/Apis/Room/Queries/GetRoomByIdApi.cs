using Core.Features.Room.Queries;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Apis.Room.Queries;

public static class GetRoomByIdApi
{
    public static void MapGetRoomByIdApi(
        this IEndpointRouteBuilder routes,
        RouteGroupBuilder group)
    {
        group.MapGet("/{id:long}",
            async (
                [FromServices] IGetRoomById getRoomById,
                [FromServices] ILogger<IGetRoomById> logger,
                long id) =>
            {
                try
                {
                    var room = await getRoomById.Handle(id);

                    return room == null ? Results.NotFound() : Results.Ok(room);
                }
                catch (Exception e)
                {
                    logger.LogError("{Exception}", e.ToString());
                    return Results.BadRequest("There was an error getting room");
                }
            });
    }
}