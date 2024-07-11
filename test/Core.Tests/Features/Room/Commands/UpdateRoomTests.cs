using Core.Domain.Dtos.Room;
using Core.Domain.Enums;
using Core.Features.Room.Commands;
using Core.Features.Room.Queries;
using Core.Repositories;
using Microsoft.Extensions.Logging;
using Moq;

namespace Core.Tests.Features.Room.Commands;

public class UpdateRoomTests
{
    private readonly UpdateRoom updateRoom;

    private readonly UpdateRoomDto dto = new()
    {
        Id = 1,
        StatusId = RoomStatusId.UnderMaintenance
    };

    private readonly Mock<IGetRoomById> getRoomById = new();
    private readonly Mock<ICommandRepository<Domain.Entities.Room>> commandRepository = new();
    private readonly Mock<ILogger<UpdateRoom>> logger = new();

    public UpdateRoomTests()
    {
        updateRoom = new(getRoomById.Object, commandRepository.Object, logger.Object);

        getRoomById.Setup(x =>
            x.Handle(It.IsAny<long>()))
            .ReturnsAsync(new Domain.Entities.Room
            {
                Number = 1
            });
    }
    
    [Fact]
    public async Task UpdateRoom_ValidDto_ReturnsRoom()
    {
        var result = await updateRoom.Handle(dto);
        // Assert
        Assert.Equal(dto.StatusId,result.StatusId);
    }

    [Fact]
    public async Task UpdateRoom_DtoIsNull_ThrowsException()
    {
        var e = await Assert.ThrowsAsync<ArgumentNullException>(
            () => updateRoom.Handle(null));

        Assert.Contains("dto", e.Message);
    }
    
    [Fact]
    public async Task UpdateRoom_RoomDoesNotExists_ThrowsException()
    {
        getRoomById.Setup(x =>
                x.Handle(It.IsAny<long>()))
            .ReturnsAsync((Domain.Entities.Room)null!);
        
        // Act & Assert
        var e = await Assert.ThrowsAsync<ArgumentNullException>(
            () => updateRoom.Handle(dto)
        );
        
        Assert.Contains("room", e.Message);
    }
}