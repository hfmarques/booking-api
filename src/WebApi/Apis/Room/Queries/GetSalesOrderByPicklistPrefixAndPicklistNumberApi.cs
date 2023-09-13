using Core.Features.Room.Queries;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Apis.Room.Queries;

public static class GetRoomsApi
{
    public static void MapGetRoomsApi(
        this IEndpointRouteBuilder routes,
        RouteGroupBuilder group)
    {
        group.MapGet("/",
            async Task<Results<Ok<IEnumerable<Core.Domain.Entities.Room>>, BadRequest<string>>>(
                [FromServices] IGetRooms getRooms,
                [FromServices] ILogger<IGetRooms> logger) =>
            {
                try
                {
                    var rooms = await getRooms.Handle();

                    return TypedResults.Ok(rooms);
                }
                catch (Exception e)
                {
                    logger.LogError("{Exception}", e.ToString());
                    return TypedResults.BadRequest("There was an error getting rooms");
                }
            });
    }
}