using Core.Domain.Entities;

namespace WebApi.Tests;

public static class GetValidCustomerToTest
{
    public static Customer Handle()
    {
        Customer customer = new()
        {
            Name = "Heber",
            Phone = "000123456789"
        };

        return customer;
    }
}