using System.Net;
using System.Net.Http.Json;
using Core.Domain.Dtos.Room;

namespace WebApi.Tests.Apis.Room.Commands;

public class PostRoomApiTests
{
    [Fact]
    public async Task PostRoomApi_ReturnsCreated()
    {
        await using var application = new WebApiApplication();
        var db = application.CreatePostgresDbContext();

        var hotel = GetValidHotelToTest.Handle();

        await db.AddAsync(hotel);
        await db.SaveChangesAsync();
        
        var dto = new AddRoomDto
        {
            Number = 1,
            HotelId = hotel.Id
        };

        var clientPostRoom = application.CreateClient();
        var response = await clientPostRoom.PostAsJsonAsync("rooms", dto);

        var clientGetRoom = application.CreateClient();
        var room = await clientGetRoom.GetFromJsonAsync<Core.Domain.Entities.Room>(
            response.Headers.Location);

        Assert.NotNull(room);
        Assert.Equal(dto.Number, room.Number);
        Assert.Equal(dto.HotelId, room.HotelId);
    }

    [Fact]
    public async Task PostRoomApi_ReturnsBadRequest()
    {
        await using var application = new WebApiApplication();
        application.CreatePostgresDbContext();

        var dto = new AddRoomDto
        {
            Number = 101,
            HotelId = 1
        };

        var clientPostRoom = application.CreateClient();
        var response = await clientPostRoom.PostAsJsonAsync("rooms", dto);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}