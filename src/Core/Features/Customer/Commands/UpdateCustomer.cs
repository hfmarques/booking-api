using Core.Domain.Dtos.Customer;
using Core.Features.Customer.Queries;
using Core.Repositories;
using Microsoft.Extensions.Logging;
using Serilog.Context;

namespace Core.Features.Customer.Commands;

public interface IUpdateCustomer
{
    Task<Domain.Entities.Customer> Handle(UpdateCustomerDto dto);
}
public class UpdateCustomer(
    IGetCustomerById getCustomerById,
    ICommandRepository<Domain.Entities.Customer> commandRepository,
    ILogger<UpdateCustomer> logger) : IUpdateCustomer
{
    public async Task<Domain.Entities.Customer> Handle(UpdateCustomerDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(dto.Id);

        var customer = await getCustomerById.Handle(dto.Id);
        
        ArgumentNullException.ThrowIfNull(customer);
        
        using var _ = LogContext.PushProperty("CorrelationId", customer.CorrelationId);
        
        logger.LogInformation("Received a customer to be updated: Customer: {Hotel}", dto);
        
        ArgumentException.ThrowIfNullOrWhiteSpace(dto.Name);
        ArgumentException.ThrowIfNullOrWhiteSpace(dto.Phone);

        customer.Name = dto.Name;
        customer.Phone = dto.Phone;
        customer.Address = dto.Address;
        
        await commandRepository.UpdateAsync();

        logger.LogInformation("Customer updated");
        
        return customer;
    }
}