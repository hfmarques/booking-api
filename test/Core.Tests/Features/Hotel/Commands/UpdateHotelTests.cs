using Core.Domain.Dtos.Hotel;
using Core.Features.Hotel.Commands;
using Core.Features.Hotel.Queries;
using Core.Repositories;
using Microsoft.Extensions.Logging;
using Moq;

namespace Core.Tests.Features.Hotel.Commands;

public class UpdateHotelTests
{
    private readonly UpdateHotel addHotel;

    private UpdateHotelDto dto = new(1, "Hotel Test");

    private readonly Mock<ICommandRepository<Domain.Entities.Hotel>> commandRepository = new();
    private readonly Mock<IGetHotelById> getHotelById = new();
    private readonly Mock<ILogger<UpdateHotel>> logger = new();

    public UpdateHotelTests()
    {
        addHotel = new(commandRepository.Object, getHotelById.Object, logger.Object);

        getHotelById.Setup(x => x.Handle(It.IsAny<long>())).ReturnsAsync(
            new Domain.Entities.Hotel
            {
                Name = "aaa",
                Rooms = []
            });
    }

    [Fact]
    public async Task UpdateHotel_ValidDto_ReturnsHotel()
    {
        await addHotel.Handle(dto);
    }

    [Fact]
    public async Task UpdateHotel_DtoIsNull_ThrowsArgumentException()
    {
        await Assert.ThrowsAsync<ArgumentNullException>(
            () => addHotel.Handle(null));
    }

    [Fact]
    public async Task UpdateHotel_IdIsNegative_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        dto = new(-1, "Hotel Test");
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => addHotel.Handle(dto));
    }

    [Fact]
    public async Task UpdateHotel_NameIsNull_ThrowsArgumentException()
    {
        // Arrange
        dto = new(1, null!);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(
            () => addHotel.Handle(dto)
        );
    }
}
