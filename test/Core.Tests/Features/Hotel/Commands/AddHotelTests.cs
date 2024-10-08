using Core.Domain.Dtos.Hotel;
using Core.Features.Hotel.Commands;
using Core.Repositories;
using Microsoft.Extensions.Logging;
using Moq;

namespace Core.Tests.Features.Hotel.Commands;

public class AddHotelTests
{
    private readonly AddHotel addHotel;

    private AddHotelDto dto = new(
        "Hotel Test",
        [
            new(101),
            new(102),
            new(103),
        ]);

    private readonly Mock<ICommandRepository<Domain.Entities.Hotel>> commandRepository = new();
    private readonly Mock<ILogger<AddHotel>> logger = new();

    public AddHotelTests()
    {
        addHotel = new(commandRepository.Object, logger.Object);
    }

    [Fact]
    public async Task AddHotel_ValidDto_ReturnsHotel()
    {
        var result = await addHotel.Handle(dto);
        // Assert
        Assert.Equal(dto.Name, result.Name);
    }

    [Fact]
    public async Task AddHotel_DtoIsNull_ThrowsArgumentException()
    {
        var e = await Assert.ThrowsAsync<ArgumentNullException>(
            () => addHotel.Handle(null));

        Assert.Contains("dto", e.Message);
    }

    [Fact]
    public async Task AddHotel_NameIsNull_ThrowsArgumentException()
    {
        // Arrange
        dto = new(null!, []);

        // Act & Assert
        var e = await Assert.ThrowsAsync<ArgumentNullException>(
            () => addHotel.Handle(dto)
        );

        Assert.Contains("Name", e.Message);
    }

    [Fact]
    public async Task AddHotel_RoomsIsNull_ThrowsArgumentException()
    {
        // Arrange
        dto = new("test", null!);

        // Act & Assert
        var e = await Assert.ThrowsAsync<ArgumentNullException>(
            () => addHotel.Handle(dto)
        );

        Assert.Contains("Rooms", e.Message);
    }

    [Fact]
    public async Task AddHotel_NoRooms_ThrowsArgumentException()
    {
        // Arrange
        dto = new("aaa", []);
        // Act & Assert
        var e = await Assert.ThrowsAsync<ArgumentException>(() => addHotel.Handle(dto));

        Assert.Contains("The hotel needs at least one room", e.Message);
    }

    [Fact]
    public async Task AddHotel_DuplicateRoomNumbers_ThrowsArgumentException()
    {
        // Arrange
        dto = new("aaa", [
            new(101),
            new(102),
            new(101),
        ]);

        // Act & Assert
        var e = await Assert.ThrowsAsync<ArgumentException>(() => addHotel.Handle(dto));

        Assert.Contains("Room numbers should not repeat", e.Message);
    }

}
