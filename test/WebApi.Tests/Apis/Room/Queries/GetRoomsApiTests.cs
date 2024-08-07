﻿using System.Net;
using System.Net.Http.Json;

namespace WebApi.Tests.Apis.Room.Queries;

public class GetRoomsApiTests
{
    [Fact]    
    public async Task GetAllRoomsApi_ReturnsOk()
    {
        await using var application = new WebApiApplication();
        var db = application.CreatePostgresDbContext();

        var hotel = GetValidHotelToTest.Handle();
        
        await db.Set<Core.Domain.Entities.Hotel>().AddAsync(hotel);
        await db.SaveChangesAsync();
        
        var client = application.CreateClient();
        var result = await client.GetFromJsonAsync<List<Core.Domain.Entities.Room>>("/rooms");

        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(hotel.Rooms.Count, result.Count);
        Assert.Equal(hotel.Rooms[0].Number, result.First(x => x.Id == hotel.Rooms[0].Id).Number);
        Assert.Equal(hotel.Rooms[1].Number, result.First(x => x.Id == hotel.Rooms[1].Id).Number);
    }
    
    [Fact]    
    public async Task GetAllRoomsApi_ReturnsNotFound()
    {
        await using var application = new WebApiApplication();
        application.CreatePostgresDbContext();

        var client = application.CreateClient();
        var result = await client.GetAsync("/rooms");

        Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
    }
}