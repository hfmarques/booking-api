using System.Net;
using System.Net.Http.Json;
using Core.Domain.Dtos.Room;
using Core.Domain.Enums;

namespace WebApi.Tests.Apis.Room.Commands;

public class PutRoomApiTests
{
    [Fact]
    public async Task PutRoomApi_ReturnsNoContent()
    {
        await using var application = new WebApiApplication();
        var db = application.CreatePostgresDbContext();

        var validHotel = GetValidHotelToTest.Handle();

        validHotel.Rooms.First().StatusId = RoomStatusId.Available;
        
        await db.AddAsync(validHotel);
        await db.SaveChangesAsync();

        var roomSaved = validHotel.Rooms.First();
        var dto = new UpdateRoomDto()
        {
            Id = roomSaved.Id,
            StatusId = RoomStatusId.UnderMaintenance
        };

        var clientPostRoom = application.CreateClient();
        var response = await clientPostRoom.PutAsJsonAsync($"rooms/id/{roomSaved.Id}", dto);
        
        response.EnsureSuccessStatusCode();

        var clientGetRoom = application.CreateClient();
        var room = await clientGetRoom.GetFromJsonAsync<Core.Domain.Entities.Room>($"/rooms/id/{roomSaved.Id}");

        Assert.NotNull(room);
        Assert.Equal(roomSaved.Id, room.Id);
        Assert.Equal(RoomStatusId.UnderMaintenance, room.StatusId);
    }

    [Fact]
    public async Task PostRoomByIdApi_ReturnsBadRequest()
    {
        await using var application = new WebApiApplication();
        application.CreatePostgresDbContext();

        var dto = new UpdateRoomDto()
        {
            Id = 123,
            StatusId = RoomStatusId.Available,
        };

        var clientPostRoom = application.CreateClient();
        var response = await clientPostRoom.PutAsJsonAsync($"rooms/id/{dto.Id}", dto);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}