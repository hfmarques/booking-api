using System.Net;
using System.Net.Http.Json;
using Core.Domain.Dtos.Hotel;

namespace WebApi.Tests.Apis.Hotel.Commands;

public class PostHotelApiTests
{
    [Fact]
    public async Task PostHotelApi_ReturnsCreated()
    {
        await using var application = new WebApiApplication();
        application.CreatePostgresDbContext();

        var dto = new AddHotelDto("Test",
            GetValidHotelToTest.Handle(50).Rooms.Select(x => new AddHotelRoomDto
            {
                Number = x.Number
            }).ToList());

        var clientPostHotel = application.CreateClient();
        var response = await clientPostHotel.PostAsJsonAsync("hotels", dto);

        response.EnsureSuccessStatusCode();

        var clientGetHotel = application.CreateClient();
        var hotel = await clientGetHotel.GetFromJsonAsync<Core.Domain.Entities.Hotel>(
            response.Headers.Location);

        Assert.NotNull(hotel);
        Assert.Equal(dto.Name, hotel.Name);
    }

    [Fact]
    public async Task PostHotelApi_ReturnsBadRequest()
    {
        await using var application = new WebApiApplication();
        application.CreatePostgresDbContext();

        var dto = new AddHotelDto("",
            GetValidHotelToTest.Handle(1).Rooms.Select(x => new AddHotelRoomDto
            {
                Number = x.Number
            }).ToList()
        );


        var clientPostHotel = application.CreateClient();
        var response = await clientPostHotel.PostAsJsonAsync("hotels", dto);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
