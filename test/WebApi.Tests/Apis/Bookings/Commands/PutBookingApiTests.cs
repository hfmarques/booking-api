using System.Net;
using System.Net.Http.Json;
using Core.Domain.Dtos.Booking;
using Core.Domain.Dtos.Hotel;

namespace WebApi.Tests.Apis.Bookings.Commands;

public class PutBookingApiTests
{
    [Fact]
    public async Task PutBookingApi_ReturnsNoContent()
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
        var response = await clientPostHotel.PutAsJsonAsync("bookings", new UpdateBookingDto
        {
            Id = booking.Id,
            StartDate = booking.StartDate,
            EndDate = booking.EndDate.AddDays(-1),
            RoomId = booking.Room!.Id,
            CustomerId = booking.Customer!.Id
        });

        response.EnsureSuccessStatusCode();
        
        var clientGetHotel = application.CreateClient();
        var bookingResponse = await clientGetHotel.GetFromJsonAsync<Core.Domain.Entities.Booking>($"bookings/id/{booking.Id}");
        
        Assert.NotNull(bookingResponse);
        Assert.Equal(booking.EndDate.AddDays(-1), bookingResponse.EndDate);
    }

    [Fact]
    public async Task PutBookingApi_ReturnsBadRequest()
    {
        await using var application = new WebApiApplication();
        application.CreatePostgresDbContext();

        var hotelDto = GetValidHotelToTest.Handle();
        var customersDto = GetValidCustomerToTest.Handle();

        var booking = GetValidBookingsToTest.Handle(hotelDto.Rooms, customersDto, 1).First();

        var clientPostHotel = application.CreateClient();
        var response = await clientPostHotel.PutAsJsonAsync("bookings",  new UpdateBookingDto
        {
            Id = booking.Id,
            StartDate = booking.StartDate,
            EndDate = booking.EndDate.AddDays(-1),
            RoomId = booking.Room!.Id,
            CustomerId = booking.Customer!.Id
        });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}