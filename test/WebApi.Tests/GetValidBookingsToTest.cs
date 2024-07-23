using Bogus;
using Core.Domain.Entities;
using Core.Domain.Enums;

namespace WebApi.Tests;

public static class GetValidBookingsToTest
{
    public static List<Booking> Handle(List<Room> rooms, List<Customer> customers, int count = 10)
    {
        var bookingFaker = new Faker<Booking>()
            .RuleFor(x => x.StartDate, f => f.Date.SoonDateOnly(30))
            .RuleFor(x => x.EndDate, (f, x) => f.Date.SoonDateOnly(3, x.StartDate))
            .RuleFor(x => x.StatusId, f => f.Random.Enum<BookingStatusId>())
            .RuleFor(x => x.Room, f => f.Random.ArrayElement(rooms.ToArray()))
            .RuleFor(x => x.Customer, f => f.Random.ArrayElement(customers.ToArray()));
        
        return bookingFaker.Generate(count);
    }
}