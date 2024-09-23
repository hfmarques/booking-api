namespace Core.Domain.Dtos.Customer;

public record UpdateCustomerDto(
    long Id,
    string Name,
    string Phone,
    string? Address = null
);
