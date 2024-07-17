using Bogus;
using Core.Domain.Entities;
using Core.Domain.Enums;

namespace WebApi.Tests;

public static class GetValidHotelToTest
{
    public static Hotel Handle()
    {
        var roomFaker = new Faker<Room>()
            .RuleFor(x => x.Number, f => f.Random.Number(100, 499))
            .RuleFor(x => x.StatusId, f => f.Random.Enum<RoomStatusId>());
        
        var hotel = new Hotel
        {
            Name = "Test",
            Rooms = roomFaker.Generate(20)
        };

        return hotel;
    }
}