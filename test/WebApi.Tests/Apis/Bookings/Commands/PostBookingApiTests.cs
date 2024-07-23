using System.Net;
using System.Net.Http.Json;
using Core.Domain.Dtos.Booking;
using Core.Domain.Dtos.Hotel;

namespace WebApi.Tests.Apis.Bookings.Commands;

public class PostBookingApiTests
{
    [Fact]
    public async Task PostBookingApi_ReturnsCreated()
    {
        await using var application = new WebApiApplication();
        var db = application.CreatePostgresDbContext();

        var hotelDto = GetValidHotelToTest.Handle();
        var customersDto = GetValidCustomerToTest.Handle();

        await db.AddAsync(hotelDto);
        await db.AddRangeAsync(customersDto);

        await db.SaveChangesAsync();
        
        var dtos = GetValidBookingsToTest.Handle(hotelDto.Rooms, customersDto, 1);

        foreach (var dto in dtos)
        {
            using var clientPostHotel = application.CreateClient();
            var response = await clientPostHotel.PostAsJsonAsync("bookings", new BookRoomDto
            {
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                RoomId = dto.Room!.Id,
                CustomerId = dto.Customer!.Id
            });
            
            var clientGetHotel = application.CreateClient();
            var booking = await clientGetHotel.GetFromJsonAsync<Core.Domain.Entities.Booking>(
                response.Headers.Location);

            Assert.NotNull(booking);
            Assert.Equal(dto.StartDate, booking.StartDate);
            Assert.Equal(dto.EndDate, booking.EndDate);
        }
    }

    [Fact]
    public async Task PostBookingApi_ReturnsBadRequest()
    {
        await using var application = new WebApiApplication();
        application.CreatePostgresDbContext();
    
        var hotelDto = GetValidHotelToTest.Handle();
        var customersDto = GetValidCustomerToTest.Handle();
        
        var dtos = GetValidBookingsToTest.Handle(hotelDto.Rooms, customersDto, 1);
    
        var clientPostHotel = application.CreateClient();
        var response = await clientPostHotel.PostAsJsonAsync("bookings", dtos.First());
    
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}