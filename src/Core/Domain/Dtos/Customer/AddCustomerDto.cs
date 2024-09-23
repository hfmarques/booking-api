namespace Core.Domain.Dtos.Customer;

public record AddCustomerDto(string Name, string Phone, string? Address = null);
