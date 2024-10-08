﻿using System.Net;
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

        var validHotel = GetValidHotelToTest.Handle(5);

        await db.Set<Core.Domain.Entities.Hotel>().AddAsync(validHotel);
        await db.SaveChangesAsync();

        var dto = new UpdateHotelDto(validHotel.Id, "Test 123");

        var clientPostHotel = application.CreateClient();
        var response = await clientPostHotel.PutAsJsonAsync($"hotels/{dto.Id}", dto);

        response.EnsureSuccessStatusCode();

        var clientGetHotel = application.CreateClient();
        var hotel = await clientGetHotel.GetFromJsonAsync<Core.Domain.Entities.Hotel>($"/hotels/{validHotel.Id}");

        Assert.NotNull(hotel);
        Assert.Equal(dto.Name, hotel.Name);
    }

    [Fact]
    public async Task PostHotelByIdApi_ReturnsBadRequest()
    {
        await using var application = new WebApiApplication();
        application.CreatePostgresDbContext();

        var dto = new UpdateHotelDto(123, "Test 123");

        var clientPostHotel = application.CreateClient();
        var response = await clientPostHotel.PutAsJsonAsync($"hotels/{dto.Id}", dto);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
