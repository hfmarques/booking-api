﻿using System.Net;
using System.Net.Http.Json;
using Core.Domain.Dtos.Customer;
using Core.Domain.Enums;

namespace WebApi.Tests.Apis.Customer.Commands;

public class PutCustomerApiTests
{
    [Fact]
    public async Task PutCustomerApi_ReturnsNoContent()
    {
        await using var application = new WebApiApplication();
        var db = application.CreatePostgresDbContext();

        var validCustomer = GetValidCustomerToTest.Handle(1);

        await db.AddRangeAsync(validCustomer);
        await db.SaveChangesAsync();

        var dto = new UpdateCustomerDto(validCustomer.First().Id,"Name 1","Phone 1");

        var clientPostCustomer = application.CreateClient();
        var response = await clientPostCustomer.PutAsJsonAsync($"customers/{validCustomer.First().Id}", dto);

        response.EnsureSuccessStatusCode();

        var clientGetCustomer = application.CreateClient();
        var customer = await clientGetCustomer.GetFromJsonAsync<Core.Domain.Entities.Customer>($"/customers/{validCustomer.First().Id}");

        Assert.NotNull(customer);
        Assert.Equal(validCustomer.First().Id, customer.Id);
        Assert.Equal("Name 1", customer.Name);
        Assert.Equal("Phone 1", customer.Phone);
    }

    [Fact]
    public async Task PostCustomerByIdApi_ReturnsBadRequest()
    {
        await using var application = new WebApiApplication();
        application.CreatePostgresDbContext();

        var dto = new UpdateCustomerDto(123,"aaaa", "   ");

        var clientPostCustomer = application.CreateClient();
        var response = await clientPostCustomer.PutAsJsonAsync($"customers/{dto.Id}", dto);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
