using System.Net;
using System.Net.Http.Json;
using Core.Domain.Dtos.Hotel;
using Microsoft.AspNetCore.Http;

namespace WebApi.Tests.Apis.Hotel.Commands;

public class PostHotelApiTests
{
    [Fact]
    public async Task PostHotelApi_ReturnsCreated()
    {
        await using var application = new WebApiApplication();
        application.CreatePostgresDbContext();

        var dto = new AddHotelDto
        {
            Name = "Test",
            Rooms =
            [
                new()
                {
                    Number = 1
                },
                new()
                {
                    Number = 2
                },
            ]
        };


        var clientPostHotel = application.CreateClient();
        var response = await clientPostHotel.PostAsJsonAsync("hotels", dto);

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

        var dto = new AddHotelDto
        {
            Name = "",
            Rooms =
            [
                new()
                {
                    Number = 1
                },
                new()
                {
                    Number = 2
                },
            ]
        };


        var clientPostHotel = application.CreateClient();
        var response = await clientPostHotel.PostAsJsonAsync("hotels", dto);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}