using WebApi.Apis.Hotel.Queries;

namespace WebApi.Apis.Hotel;

public static class HotelApiMap
{
    public static void AddApisFromHotels(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/hotels")
            .WithTags("Hotel");

        app.MapGetHotelsApi(group);
        app.MapGetHotelByIdApi(group);
    }
}