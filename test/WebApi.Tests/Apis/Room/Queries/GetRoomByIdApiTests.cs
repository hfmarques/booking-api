using System.Net;
using System.Net.Http.Json;
using Core.Domain.Entities;

namespace WebApi.Tests.Apis.Room.Queries;

public class GetRoomByIdApiTests
{
    [Fact]    
    public async Task GetAllRoomByIdApi_ReturnsOk()
    {
        await using var application = new WebApiApplication();
        var db = application.CreatePostgresDbContext();

        var hotel = GetValidHotelToTest.Handle();
        
        await db.Set<Core.Domain.Entities.Hotel>().AddAsync(hotel);
        await db.SaveChangesAsync();
        
        var client = application.CreateClient();
        var result = await client.GetFromJsonAsync<Core.Domain.Entities.Room>($"/rooms/id/{hotel.Rooms.First().Id}");

        Assert.NotNull(result);
        Assert.Equal(hotel.Rooms.First().Id, result.Id);
        Assert.Equal(hotel.Rooms.First().Number, result.Number);
        Assert.Equal(hotel.Rooms.First().HotelId, result.HotelId);
    }
    
    [Fact]    
    public async Task GetAllRoomByIdApi_ReturnsNotFound()
    {
        await using var application = new WebApiApplication();
        var db = application.CreatePostgresDbContext();

        var hotel = GetValidHotelToTest.Handle();
        
        await db.Set<Core.Domain.Entities.Hotel>().AddAsync(hotel);
        await db.SaveChangesAsync();
        
        var client = application.CreateClient();
        var result = await client.GetAsync($"/rooms/id/{123}");

        Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
    }
}