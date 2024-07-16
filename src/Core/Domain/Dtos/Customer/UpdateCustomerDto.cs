namespace Core.Domain.Dtos.Customer;

public class UpdateCustomerDto
{
    public required long Id { get; set; }
    public required string Name { get; set; }
    public required string Phone { get; set; }
    public string? Address { get; set; }   
}