using System.Net;
using System.Net.Http.Json;
using Core.Domain.Dtos.Booking;
using Core.Domain.Dtos.Hotel;
using Core.Domain.Enums;

namespace WebApi.Tests.Apis.Bookings.Commands;

public class DeleteBookingApiTests
{
    [Fact]
    public async Task DeleteBookingApi_ReturnsNoContent()
    {
        await using var application = new WebApiApplication();
        var db = application.CreatePostgresDbContext();

        var hotelDto = GetValidHotelToTest.Handle();
        var customersDto = GetValidCustomerToTest.Handle();

        await db.AddAsync(hotelDto);
        await db.AddRangeAsync(customersDto);

        await db.SaveChangesAsync();
        
        var booking = GetValidBookingsToTest.Handle(hotelDto.Rooms, customersDto, 1).First();

        await db.AddAsync(booking);
        
        await db.SaveChangesAsync();

        using var clientPostHotel = application.CreateClient();
        var response = await clientPostHotel.DeleteAsync($"bookings/{booking.Id}");

        response.EnsureSuccessStatusCode();
        
        var clientGetHotel = application.CreateClient();
        var bookingResponse = await clientGetHotel.GetFromJsonAsync<Core.Domain.Entities.Booking>($"bookings/{booking.Id}");
        
        Assert.NotNull(bookingResponse);
        Assert.Equal(BookingStatusId.Cancelled, bookingResponse.StatusId);
    }

    [Fact]
    public async Task DeleteBookingApi_ReturnsBadRequest()
    {
        await using var application = new WebApiApplication();
        application.CreatePostgresDbContext();

        var clientPostHotel = application.CreateClient();
        var response = await clientPostHotel.DeleteAsync("bookings/10");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}