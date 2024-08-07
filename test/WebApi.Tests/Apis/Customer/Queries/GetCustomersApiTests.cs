﻿using System.Net;
using System.Net.Http.Json;

namespace WebApi.Tests.Apis.Customer.Queries;

public class GetCustomersApiTests
{
    [Fact]    
    public async Task GetAllCustomersApi_ReturnsOk()
    {
        await using var application = new WebApiApplication();
        var db = application.CreatePostgresDbContext();

        var customers = GetValidCustomerToTest.Handle();
        
        await db.Set<Core.Domain.Entities.Customer>().AddRangeAsync(customers);
        await db.SaveChangesAsync();
        
        var client = application.CreateClient();
        var result = await client.GetFromJsonAsync<List<Core.Domain.Entities.Customer>>("/customers");

        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(customers.Count, result.Count);
    }
    
    [Fact]    
    public async Task GetAllCustomersApi_ReturnsNotFound()
    {
        await using var application = new WebApiApplication();
        application.CreatePostgresDbContext();

        var client = application.CreateClient();
        var result = await client.GetAsync("/customers");

        Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
    }
}