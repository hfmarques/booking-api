using System.Net;
using System.Net.Http.Json;

namespace WebApi.Tests.Apis.Bookings.Queries;

public class GetUpcomingBookingsApiTests
{
    [Fact]    
    public async Task GetUpcomingBookingsApi_ReturnsOk()
    {
        await using var application = new WebApiApplication();
        var db = application.CreatePostgresDbContext();

        var hotel = GetValidHotelToTest.Handle();
        var customers = GetValidCustomerToTest.Handle();

        await db.Set<Core.Domain.Entities.Hotel>().AddAsync(hotel);
        await db.Set<Core.Domain.Entities.Customer>().AddRangeAsync(customers);
        
        var bookings = GetValidBookingsToTest.Handle(hotel.Rooms, customers);
        
        await db.Set<Core.Domain.Entities.Booking>().AddRangeAsync(bookings);
        await db.SaveChangesAsync();
        
        var client = application.CreateClient();
        var result = await client.GetFromJsonAsync<List<Core.Domain.Entities.Booking>>("/bookings/upcoming");

        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(bookings.Count(x => x.StartDate > DateOnly.FromDateTime(DateTime.Now)), result.Count);
    }
    
    [Fact]    
    public async Task GetUpcomingBookingsApi_ReturnsNotFound()
    {
        await using var application = new WebApiApplication();
        application.CreatePostgresDbContext();

        var client = application.CreateClient();
        var result = await client.GetAsync("/bookings/upcoming");

        Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
    }
}