using System.Net;
using System.Net.Http.Json;

namespace WebApi.Tests.Apis.Bookings.Queries;

public class GetBookingsByIdApiTests
{
    [Fact]    
    public async Task GetAllBookingsByIdApi_ReturnsOk()
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
        var result = await client.GetFromJsonAsync<Core.Domain.Entities.Booking>($"/bookings/id/{bookings.First().Id}");

        Assert.NotNull(result);
        Assert.Equal(bookings.First().Id, result.Id);
    }
    
    [Fact]    
    public async Task GetAllBookingsByIdApi_ReturnsNotFound()
    {
        await using var application = new WebApiApplication();
        var db = application.CreatePostgresDbContext();
       
        var client = application.CreateClient();
        var result = await client.GetAsync($"/bookings/id/{123}");

        Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
    }
}