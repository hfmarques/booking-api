using Core.Domain.Entities;

namespace WebApi.Tests;

public static class GetValidHotelToTest
{
    public static Hotel Handle()
    {
        var room1 = new Room
        {
            Number = 1
        };
        var room2 = new Room
        {
            Number = 2
        };
        var hotel = new Hotel()
        {
            Name = "Test",
            Rooms = [room1, room2]
        };

        return hotel;
    }
}