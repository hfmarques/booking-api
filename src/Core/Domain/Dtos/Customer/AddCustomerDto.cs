namespace Core.Domain.Dtos.Customer;

public class AddCustomerDto
{
    public required string Name { get; set; }
    public required string Phone { get; set; }
    public string? Address { get; set; }   
}