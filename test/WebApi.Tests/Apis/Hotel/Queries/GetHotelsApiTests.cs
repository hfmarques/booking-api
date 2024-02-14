using System.Net;
using System.Net.Http.Json;

namespace WebApi.Tests.Apis.Hotel.Queries;

public class GetHotelsApiTests
{
    [Fact]    
    public async Task GetAllHotelsApi_ReturnsOk()
    {
        await using var application = new WebApiApplication();
        var db = application.CreatePostgresDbContext();

        var hotel = GetValidHotelToTest.Handle();
        
        await db.Set<Core.Domain.Entities.Hotel>().AddAsync(hotel);
        await db.SaveChangesAsync();
        
        var client = application.CreateClient();
        var result = await client.GetFromJsonAsync<List<Core.Domain.Entities.Hotel>>("/hotels");

        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Single(result);
        Assert.Equal(hotel.Name, result.First().Name);
    }
    
    [Fact]    
    public async Task GetAllHotelsApi_ReturnsNotFound()
    {
        await using var application = new WebApiApplication();
        application.CreatePostgresDbContext();

        var client = application.CreateClient();
        var result = await client.GetAsync("/hotels");

        Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
    }
}