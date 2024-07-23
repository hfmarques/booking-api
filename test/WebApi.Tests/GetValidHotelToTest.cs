using Bogus;
using Core.Domain.Entities;
using Core.Domain.Enums;

namespace WebApi.Tests;

public static class GetValidHotelToTest
{
    public static Hotel Handle(int count = 5)
    {
        var roomNumber = 100;
        var roomFaker = new Faker<Room>()
            .RuleFor(x => x.Number, f => roomNumber++)
            .RuleFor(x => x.StatusId, f => f.Random.Enum<RoomStatusId>());
        
        var hotel = new Hotel
        {
            Name = "Test",
            Rooms = roomFaker.Generate(count)
        };

        return hotel;
    }
}