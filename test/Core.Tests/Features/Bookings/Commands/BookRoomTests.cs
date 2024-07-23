using Core.Domain.Dtos.Booking;
using Core.Domain.Entities;
using Core.Features.Booking.Commands;
using Core.Repositories;
using Microsoft.Extensions.Logging;
using Moq;

namespace Core.Tests.Features.Bookings.Commands;

public class BookRoomTests
{
    private readonly BookRoom bookRoom;

    private readonly Mock<IBookingQueryRepository> queryRepository = new();
    private readonly Mock<ICommandRepository<Domain.Entities.Booking>> commandRepository = new();
    private readonly Mock<IVerifyBookingAvailability> verifyBookingAvailability = new();
    private readonly Mock<ILogger<BookRoom>> logger = new();

    private readonly BookRoomDto dto = new()
    {
        StartDate = DateOnly.FromDateTime(DateTime.Now),
        EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
        RoomId = 1,
        CustomerId = 1
    };

    public BookRoomTests()
    {
        bookRoom= new(
            queryRepository.Object,
            commandRepository.Object,
            verifyBookingAvailability.Object,
            logger.Object);

        verifyBookingAvailability.Setup(x => x.Handle(
                It.IsAny<Booking>(), It.IsAny<IReadOnlyCollection<Booking>>()))
            .ReturnsAsync(true);
    }

    [Fact]
    public async Task BookRoom_DtoIsNull_ThrowException()
    {
        var e = await Assert.ThrowsAsync<ArgumentNullException>(() => bookRoom.Handle(null!));

        Assert.Contains("dto", e.Message);
    }

    [Fact]
    public async Task BookRoom_BookingNotAvailable_ThrowException()
    {
        verifyBookingAvailability.Setup(x => x.Handle(
                It.IsAny<Booking>(), It.IsAny<IReadOnlyCollection<Booking>>()))
            .ReturnsAsync(false);

        var e = await Assert.ThrowsAsync<ArgumentException>(() => bookRoom.Handle(dto));
        
        Assert.Contains("date not available", e.Message);
    }
    
    [Fact]
    public async Task BookRoom_BookingAvailable_ReturnsBook()
    {
        var result = await bookRoom.Handle(dto);
        
        Assert.NotNull(result);
    }
}