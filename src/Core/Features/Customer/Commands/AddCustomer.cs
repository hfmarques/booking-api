using Core.Domain.Dtos.Customer;
using Core.Repositories;
using Microsoft.Extensions.Logging;
using Serilog.Context;

namespace Core.Features.Customer.Commands;

public interface IAddCustomer
{
    Task<Domain.Entities.Customer> Handle(AddCustomerDto dto);
}
public class AddCustomer(
    ICustomerQueryRepository customerQueryRepository,
    ICommandRepository<Domain.Entities.Customer> commandRepository,
    ILogger<AddCustomer> logger) : IAddCustomer
{
    public async Task<Domain.Entities.Customer> Handle(AddCustomerDto dto)
    {
        var correlationId = Guid.NewGuid();
        using var _ = LogContext.PushProperty("CorrelationId", correlationId);

        logger.LogInformation("Received a new customer to be saved. Customer: {Customer}", dto);
        
        ArgumentNullException.ThrowIfNull(dto);
        ArgumentException.ThrowIfNullOrWhiteSpace(dto.Name);
        ArgumentException.ThrowIfNullOrWhiteSpace(dto.Phone);

        var customerDb = await customerQueryRepository.FindAsync(x => 
            x.Name == dto.Name &&
            x.Phone == dto.Phone);
        if (customerDb.Any())
        {
            logger.LogWarning("Customer Already exists");
            throw new ArgumentException("Customer Already exists");
        }
        
        logger.LogInformation("Customer data is fine, inserting into database");

        var customer = new Domain.Entities.Customer
        {
            Name = dto.Name,
            Phone = dto.Phone,
            CorrelationId = correlationId
        };

        await commandRepository.AddAsync(customer);
        
        logger.LogInformation("Customer inserted into database");

        return customer;
    }
}