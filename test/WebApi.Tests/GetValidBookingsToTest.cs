using Bogus;
using Core.Domain.Entities;
using Model.Enum;

namespace WebApi.Tests;

public static class GetValidBookingsToTest
{
    public static List<Booking> Handle(List<Room> rooms, List<Customer> customers)
    {
        var bookingFaker = new Faker<Booking>()
            .RuleFor(x => x.StartDate, f => f.Date.SoonDateOnly(30))
            .RuleFor(x => x.EndDate, f => f.Date.SoonDateOnly(45))
            .RuleFor(x => x.StatusId, f => f.Random.Enum<BookingStatusId>())
            .RuleFor(x => x.Room, f => f.Random.ArrayElement(rooms.ToArray()))
            .RuleFor(x => x.Customer, f => f.Random.ArrayElement(customers.ToArray()));
        
        return bookingFaker.Generate(100);
    }
}