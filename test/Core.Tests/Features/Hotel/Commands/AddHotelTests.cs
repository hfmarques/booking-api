using Core.Domain.Dtos.Hotel;
using Core.Domain.Dtos.Room;
using Core.Features.Hotel.Commands;
using Core.Repositories;
using Moq;

namespace Core.Tests.Features.Hotel.Commands;

public class AddHotelTests
{
    private readonly AddHotel addHotel;

    private AddHotelDto dto = new()
    {
        Name = "Hotel Test",
        Rooms =
        [
            new() {Number = 101},
            new() {Number = 102},
            new() {Number = 103},
        ]
    };

    private readonly Mock<ICommandRepository<Domain.Entities.Hotel>> commandRepository = new();

    public AddHotelTests()
    {
        addHotel = new(commandRepository.Object);
    }
    
    [Fact]
    public async Task AddHotel_ValidDto_ReturnsHotel()
    {
        var result = await addHotel.Handle(dto);
        // Assert
        Assert.Equal(dto.Name,result.Name);
    }

    [Fact]
    public async Task AddHotel_DtoIsNull_ThrowsArgumentException()
    {
        await Assert.ThrowsAsync<ArgumentNullException>(
            () => addHotel.Handle(null));
    }
    
    [Fact]
    public async Task AddHotel_NameIsNull_ThrowsArgumentException()
    {
        // Arrange
        dto.Name = null;

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(
            () => addHotel.Handle(dto)
        ); 
    }
    
    [Fact]
    public async Task AddHotel_RoomsIsNull_ThrowsArgumentException()
    {
        // Arrange
        dto.Rooms = null;

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(
            () => addHotel.Handle(dto)
        );  
    }
    
    [Fact]
    public async Task AddHotel_NoRooms_ThrowsArgumentException()
    {
        // Arrange
        dto.Rooms = [];
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => addHotel.Handle(dto));
    }
    
    [Fact]
    public async Task AddHotel_DuplicateRoomNumbers_ThrowsArgumentException()
    {
        // Arrange
        dto.Rooms = new()
        {
            new()
            {
                Number = 101
            },
            new()
            {
                Number = 102
            },
            new()
            {
                Number = 101
            }
        };
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => addHotel.Handle(dto));
    }
}