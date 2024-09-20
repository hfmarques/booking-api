using Core.Features.Room.Queries;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Apis.Room.Queries;

public static class GetRoomsApi
{
    public static void MapGetRoomsApi(
        this IEndpointRouteBuilder routes,
        RouteGroupBuilder group)
    {
        group.MapGet("/",
            async (
                [FromServices] IGetRooms getRooms,
                [FromServices] ILogger<IGetRooms> logger) =>
            {
                try
                {
                    var rooms = await getRooms.Handle();

                    return !rooms.Any() ? Results.NotFound(new List<Core.Domain.Entities.Room>()) : Results.Ok(rooms);
                }
                catch (Exception e)
                {
                    logger.LogError("{Exception}", e.ToString());
                    return Results.BadRequest("There was an error getting rooms");
                }
            });
    }
}
