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
        var db = application.CreatePostgresDbContext();

        var dto = new AddCustomerDto
        {
            Name = "Test",
            Phone = "0000123456498"
        };

        var clientPostCustomer = application.CreateClient();
        var response = await clientPostCustomer.PostAsJsonAsync("customers", dto);

        var clientGetCustomer = application.CreateClient();
        var customer = await clientGetCustomer.GetFromJsonAsync<Core.Domain.Entities.Customer>(
            response.Headers.Location);

        Assert.NotNull(customer);
        Assert.Equal(dto.Name, customer.Name);
        Assert.Equal(dto.Phone, customer.Phone);
    }

    [Fact]
    public async Task PostCustomerApi_ReturnsBadRequest()
    {
        await using var application = new WebApiApplication();
        application.CreatePostgresDbContext();

        var dto = new AddCustomerDto
        {
            Name = "Test",
            Phone = "  "
        };

        var clientPostCustomer = application.CreateClient();
        var response = await clientPostCustomer.PostAsJsonAsync("customers", dto);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}