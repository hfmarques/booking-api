using System.Net;
using System.Net.Http.Json;
using Core.Domain.Dtos.Hotel;

namespace WebApi.Tests.Apis.Hotel.Commands;

public class PutHotelApiTests
{
    [Fact]
    public async Task PutHotelApi_ReturnsNoContent()
    {
        await using var application = new WebApiApplication();
        var db = application.CreatePostgresDbContext();

        var validHotel = GetValidHotelToTest.Handle();
        
        await db.Set<Core.Domain.Entities.Hotel>().AddAsync(validHotel);
        await db.SaveChangesAsync();

        var dto = new UpdateHotelDto()
        {
            Id = validHotel.Id,
            Name = "Test 123",
        };

        var clientPostHotel = application.CreateClient();
        await clientPostHotel.PutAsJsonAsync($"hotels/id/{dto.Id}", dto);

        var clientGetHotel = application.CreateClient();
        var hotel = await clientGetHotel.GetFromJsonAsync<Core.Domain.Entities.Hotel>($"/hotels/id/{validHotel.Id}");

        Assert.NotNull(hotel);
        Assert.Equal(dto.Name, hotel.Name);
    }

    [Fact]
    public async Task PostHotelByIdApi_ReturnsBadRequest()
    {
        await using var application = new WebApiApplication();
        application.CreatePostgresDbContext();

        var dto = new UpdateHotelDto()
        {
            Id = 123,
            Name = "Test 123",
        };

        var clientPostHotel = application.CreateClient();
        var response = await clientPostHotel.PutAsJsonAsync($"hotels/id/{dto.Id}", dto);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}