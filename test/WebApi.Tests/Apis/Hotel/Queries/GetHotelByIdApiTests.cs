using System.Net;
using System.Net.Http.Json;

namespace WebApi.Tests.Apis.Hotel.Queries;

public class GetHotelByIdApiTests
{
    [Fact]    
    public async Task GetAllHotelByIdApi_ReturnsOk()
    {
        await using var application = new WebApiApplication();
        var db = application.CreatePostgresDbContext();

        var hotel = GetValidHotelToTest.Handle();
        
        await db.Set<Core.Domain.Entities.Hotel>().AddAsync(hotel);
        await db.SaveChangesAsync();
        
        var client = application.CreateClient();
        var result = await client.GetFromJsonAsync<Core.Domain.Entities.Hotel>($"/hotels/{hotel.Id}");

        Assert.NotNull(result);
        Assert.Equal(hotel.Id, result.Id);
        Assert.Equal(hotel.Name, result.Name);
    }
    
    [Fact]    
    public async Task GetAllHotelByIdApi_ReturnsNotFound()
    {
        await using var application = new WebApiApplication();
        var db = application.CreatePostgresDbContext();

        var hotel = GetValidHotelToTest.Handle();
        
        await db.Set<Core.Domain.Entities.Hotel>().AddAsync(hotel);
        await db.SaveChangesAsync();
        
        var client = application.CreateClient();
        var result = await client.GetAsync($"/hotels/{123}");

        Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
    }
}