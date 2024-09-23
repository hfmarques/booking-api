using Core.Domain.Dtos.Room;
using Core.Features.Hotel.Queries;
using Core.Features.Room.Commands;
using Core.Repositories;
using Microsoft.Extensions.Logging;
using Moq;

namespace Core.Tests.Features.Room.Commands;

public class AddRoomTests
{
    private readonly AddRoom addRoom;

    private AddRoomDto dto = new(2, 1);

    private readonly Mock<IGetHotelById> getHotelById = new();
    private readonly Mock<ICommandRepository<Domain.Entities.Room>> commandRepository = new();
    private readonly Mock<ILogger<AddRoom>> logger = new();

    public AddRoomTests()
    {
        addRoom = new(getHotelById.Object, commandRepository.Object, logger.Object);

        getHotelById.Setup(x =>
            x.Handle(It.IsAny<long>()))
            .ReturnsAsync(new Domain.Entities.Hotel
            {
                Name = "Test",
                Rooms = [
                    new()
                    {
                        Number = 101
                    }
                ]
            });
    }

    [Fact]
    public async Task AddRoom_ValidDto_ReturnsRoom()
    {
        var result = await addRoom.Handle(dto);
        // Assert
        Assert.Equal(dto.Number,result.Number);
    }

    [Fact]
    public async Task AddRoom_DtoIsNull_ThrowsException()
    {
        var e = await Assert.ThrowsAsync<ArgumentNullException>(
            () => addRoom.Handle(null));

        Assert.Contains("dto", e.Message);
    }

    [Fact]
    public async Task AddRoom_NumberIsInvalid_ThrowsException()
    {
        // Arrange
        dto = new(0, 1);
        // Act & Assert
        var e = await Assert.ThrowsAsync<ArgumentOutOfRangeException>(
            () => addRoom.Handle(dto)
        );

        Assert.Contains("Number", e.Message);
    }

    [Fact]
    public async Task AddRoom_HotelIdIsInvalid_ThrowsException()
    {
        // Arrange
        dto = new(2, 0);
        // Act & Assert
        var e = await Assert.ThrowsAsync<ArgumentOutOfRangeException>(
            () => addRoom.Handle(dto)
        );

        Assert.Contains("HotelId", e.Message);
    }

    [Fact]
    public async Task AddRoom_HotelDoesNotExist_ThrowsException()
    {
        getHotelById.Setup(x =>
                x.Handle(It.IsAny<long>()))
            .ReturnsAsync((Domain.Entities.Hotel)null!);

        var e = await Assert.ThrowsAsync<ArgumentNullException>(() => addRoom.Handle(dto));

        Assert.Contains("hotel", e.Message);
    }

    [Fact]
    public async Task AddRoom_RoomAlreadyExists_ThrowsException()
    {
        dto = new(101, 1);
        var e = await Assert.ThrowsAsync<ArgumentException>(() => addRoom.Handle(dto));

        Assert.Contains("This hotel already has a room with this number", e.Message);
    }
}
