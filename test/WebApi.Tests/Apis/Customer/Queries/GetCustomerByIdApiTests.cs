using System.Net;
using System.Net.Http.Json;

namespace WebApi.Tests.Apis.Customer.Queries;

public class GetCustomerByIdApiTests
{
    [Fact]    
    public async Task GetAllCustomerByIdApi_ReturnsOk()
    {
        await using var application = new WebApiApplication();
        var db = application.CreatePostgresDbContext();

        var customers = GetValidCustomerToTest.Handle();
        
        await db.Set<Core.Domain.Entities.Customer>().AddRangeAsync(customers);
        await db.SaveChangesAsync();
        
        var client = application.CreateClient();
        var result = await client.GetFromJsonAsync<Core.Domain.Entities.Customer>($"/customers/id/{customers.First().Id}");

        Assert.NotNull(result);
        Assert.Equal(customers.First().Id, result.Id);
        Assert.Equal(customers.First().Name, result.Name);
    }
    
    [Fact]    
    public async Task GetAllCustomerByIdApi_ReturnsNotFound()
    {
        await using var application = new WebApiApplication();
        var db = application.CreatePostgresDbContext();

        var customers = GetValidCustomerToTest.Handle();
        
        await db.Set<Core.Domain.Entities.Customer>().AddRangeAsync(customers);
        await db.SaveChangesAsync();
        
        var client = application.CreateClient();
        var result = await client.GetAsync($"/customers/id/{123}");

        Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
    }
}