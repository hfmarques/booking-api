using Bogus;
using Core.Domain.Entities;

namespace WebApi.Tests;

public static class GetValidCustomerToTest
{
    public static List<Customer> Handle(int count = 5)
    {
        var customerFaker = new Faker<Customer>()
            .RuleFor(x => x.Name, f => f.Person.FullName)
            .RuleFor(x => x.Phone, f => f.Person.Phone)
            .RuleFor(x => x.Address, f => 
                $"{f.Person.Address.Street} - {f.Person.Address.City} - {f.Person.Address.State} - {f.Person.Address.ZipCode}");

        return customerFaker.Generate(count);
    }
}