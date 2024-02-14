using System.Net;
using System.Net.Http.Json;
using Core.Domain.Entities;

namespace WebApi.Tests.Apis.Room.Queries;

public class GetRoomsApiTests
{
    [Fact]    
    public async Task GetAllRoomsApi_ReturnsOk()
    {
        await using var application = new WebApiApplication();
        var db = application.CreatePostgresDbContext();

        var room1 = new Core.Domain.Entities.Room
        {
            Number = 1
        };
        var room2 = new Core.Domain.Entities.Room
        {
            Number = 2
        };
        var hotel = new Hotel()
        {
            Name = "Test",
            Rooms = [room1, room2]
        };
        
        await db.Set<Hotel>().AddAsync(hotel);
        await db.SaveChangesAsync();
        
        var client = application.CreateClient();
        var result = await client.GetFromJsonAsync<List<Core.Domain.Entities.Room>>("/rooms");

        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(2, result.Count);
        Assert.Equal(room1.Number, result.First(x => x.Id == room1.Id).Number);
        Assert.Equal(room2.Number, result.First(x => x.Id == room2.Id).Number);
    }
    
    [Fact]    
    public async Task GetAllRoomsApi_ReturnsNotFound()
    {
        await using var application = new WebApiApplication();
        application.CreatePostgresDbContext();

        var client = application.CreateClient();
        var result = await client.GetAsync("/rooms");

        Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
    }
}