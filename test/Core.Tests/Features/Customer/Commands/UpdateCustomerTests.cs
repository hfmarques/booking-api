using Core.Domain.Dtos.Customer;
using Core.Features.Customer.Commands;
using Core.Features.Customer.Queries;
using Core.Repositories;
using Microsoft.Extensions.Logging;
using Moq;

namespace Core.Tests.Features.Customer.Commands;

public class UpdateCustomerTests
{
    private readonly UpdateCustomer addCustomer;

    private readonly UpdateCustomerDto dto = new()
    {
        Id = 1,
        Name = "Customer Test",
        Phone = "000123456879"
    };

    private readonly Mock<ICommandRepository<Domain.Entities.Customer>> commandRepository = new();
    private readonly Mock<IGetCustomerById> getCustomerById = new();
    private readonly Mock<ILogger<UpdateCustomer>> logger = new();

    public UpdateCustomerTests()
    {
        addCustomer = new(getCustomerById.Object, commandRepository.Object, logger.Object);

        getCustomerById.Setup(x => x.Handle(It.IsAny<long>())).ReturnsAsync(
            new Domain.Entities.Customer
            {
                Name = "aaa",
                Phone = "000123456879"
            });
    }
    
    [Fact]
    public async Task UpdateCustomer_ValidDto_ReturnsCustomer()
    {
        await addCustomer.Handle(dto);
    }

    [Fact]
    public async Task UpdateCustomer_DtoIsNull_ThrowsArgumentException()
    {
        var e = await Assert.ThrowsAsync<ArgumentNullException>(
            () => addCustomer.Handle(null!));
        
        Assert.Contains("dto", e.Message);
    }
    
    [Fact]
    public async Task UpdateCustomer_IdIsNegative_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        dto.Id = -1;
        // Act & Assert
        var e = await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => addCustomer.Handle(dto));
        
        Assert.Contains("Id", e.Message);
    }
    
    [Fact]
    public async Task UpdateCustomer_GetCustomerReturnsNull_ThrowsArgumentException()
    {
        // Arrange
        getCustomerById.Setup(x => x.Handle(It.IsAny<long>())).ReturnsAsync(
            (Domain.Entities.Customer)null!);
        
        // Act & Assert
        var e = await Assert.ThrowsAsync<ArgumentNullException>(() => addCustomer.Handle(dto));
        
        Assert.Contains("customer", e.Message);
    }
    
    [Fact]
    public async Task UpdateCustomer_NameIsNull_ThrowsArgumentException()
    {
        // Arrange
        dto.Name = null!;

        // Act & Assert
        var e = await Assert.ThrowsAsync<ArgumentNullException>(
            () => addCustomer.Handle(dto)
        );
        
        Assert.Contains("Name", e.Message);
    }
    
    [Fact]
    public async Task UpdateCustomer_PhoneIsNull_ThrowsArgumentException()
    {
        // Arrange
        dto.Phone = null!;

        // Act & Assert
        var e = await Assert.ThrowsAsync<ArgumentNullException>(
            () => addCustomer.Handle(dto)
        );

        Assert.Contains("Phone", e.Message);
    }
}