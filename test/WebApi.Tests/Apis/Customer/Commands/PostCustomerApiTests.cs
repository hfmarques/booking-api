using System.Net;
using System.Net.Http.Json;
using Core.Domain.Dtos.Customer;

namespace WebApi.Tests.Apis.Customer.Commands;

public class PostCustomerApiTests
{
    [Fact]
    public async Task PostCustomerApi_ReturnsCreated()
    {
        await using var application = new WebApiApplication();
        application.CreatePostgresDbContext();

        var dtos = GetValidCustomerToTest.Handle(20);

        foreach (var dto in dtos)
        {
            using var clientPostCustomer = application.CreateClient();
            using var response = await clientPostCustomer.PostAsJsonAsync("customers", dto);

            response.EnsureSuccessStatusCode();

            var clientGetCustomer = application.CreateClient();
            var customer = await clientGetCustomer.GetFromJsonAsync<Core.Domain.Entities.Customer>(
                response.Headers.Location);

            Assert.NotNull(customer);
            Assert.Equal(dto.Name, customer.Name);
            Assert.Equal(dto.Phone, customer.Phone);
        }
    }

    [Fact]
    public async Task PostCustomerApi_ReturnsBadRequest()
    {
        await using var application = new WebApiApplication();
        application.CreatePostgresDbContext();

        var dto = new AddCustomerDto("Test", "  ");

        var clientPostCustomer = application.CreateClient();
        var response = await clientPostCustomer.PostAsJsonAsync("customers", dto);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
