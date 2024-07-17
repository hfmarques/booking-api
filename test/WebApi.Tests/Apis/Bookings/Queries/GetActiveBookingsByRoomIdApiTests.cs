using System.Net;
using System.Net.Http.Json;
using Model.Enum;

namespace WebApi.Tests.Apis.Bookings.Queries;

public class GetActiveBookingsByRoomIdApiTests
{
    [Fact]    
    public async Task GetActiveBookingsByRoomIdApi_ReturnsOk()
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
        var result = await client.GetFromJsonAsync<List<Core.Domain.Entities.Booking>>($"/bookings/active/roomId/{bookings.First().RoomId}");

        Assert.NotNull(result);
        Assert.Equal(bookings.First().RoomId, result.First().RoomId);
        Assert.Equal(bookings.Count(x => x.StatusId == BookingStatusId.Confirmed && 
                                         x.RoomId == bookings.First().RoomId), result.Count);
    }
    
    [Fact]    
    public async Task GetActiveBookingsByRoomIdApi_ReturnsNotFound()
    {
        await using var application = new WebApiApplication();
        var db = application.CreatePostgresDbContext();
        
        var client = application.CreateClient();
        var result = await client.GetAsync($"/bookings/active/roomId/{123}");

        Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
    }
}