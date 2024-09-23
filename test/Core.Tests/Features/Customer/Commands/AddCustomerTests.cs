using System.Linq.Expressions;
using Core.Domain.Dtos.Customer;
using Core.Features.Customer.Commands;
using Core.Repositories;
using Microsoft.Extensions.Logging;
using Moq;

namespace Core.Tests.Features.Customer.Commands;

public class AddCustomerTests
{
    private readonly AddCustomer addCustomer;

    private AddCustomerDto dto = new("Customer Test", "000123456789");

    private readonly Mock<ICustomerQueryRepository> customerQueryRepository = new();
    private readonly Mock<ICommandRepository<Domain.Entities.Customer>> commandRepository = new();
    private readonly Mock<ILogger<AddCustomer>> logger = new();

    public AddCustomerTests()
    {
        addCustomer = new(customerQueryRepository.Object, commandRepository.Object, logger.Object);
    }

    [Fact]
    public async Task AddCustomer_ValidDto_ReturnsCustomer()
    {
        var result = await addCustomer.Handle(dto);
        // Assert
        Assert.Equal(dto.Name, result.Name);
    }

    [Fact]
    public async Task AddCustomer_DtoIsNull_ThrowsArgumentException()
    {
        var e = await Assert.ThrowsAsync<ArgumentNullException>(
            () => addCustomer.Handle(null!));

        Assert.Contains("dto", e.Message);
    }

    [Fact]
    public async Task AddCustomer_NameIsNull_ThrowsArgumentException()
    {
        // Arrange
        dto = new(null!, "000123456789");
        // Act & Assert
        var e = await Assert.ThrowsAsync<ArgumentNullException>(
            () => addCustomer.Handle(dto)
        );

        Assert.Contains("Name", e.Message);
    }

    [Fact]
    public async Task AddCustomer_PhoneIsNull_ThrowsArgumentException()
    {
        // Arrange
        dto = new("Customer Test", null!);
        // Act & Assert
        var e = await Assert.ThrowsAsync<ArgumentNullException>(
            () => addCustomer.Handle(dto)
        );

        Assert.Contains("Phone", e.Message);
    }

    [Fact]
    public async Task AddCustomer_DuplicateCustomer_ThrowsArgumentException()
    {
        customerQueryRepository.Setup(x => x.FindAsync(
                It.IsAny<Expression<Func<Domain.Entities.Customer, bool>>>()))
            .ReturnsAsync([
                new()
                {
                    Name = "Test",
                    Phone = "000123"
                }
            ]);
        // Act & Assert
        var e = await Assert.ThrowsAsync<ArgumentException>(() => addCustomer.Handle(dto));

        Assert.Contains("Customer Already exists", e.Message);
    }
}
