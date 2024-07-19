using System.Linq.Expressions;
using Core.Domain.Dtos.Booking;
using Core.Domain.Entities;
using Core.Features.Booking.Commands;
using Core.Repositories;
using Microsoft.Extensions.Logging;
using Moq;

namespace Core.Tests.Features.Bookings.Commands;

public class UpdateBookingTests
{
    private readonly UpdateBooking updateBooking;

    private readonly Mock<IBookingQueryRepository> queryRepository = new();
    private readonly Mock<ICommandRepository<Domain.Entities.Booking>> commandRepository = new();
    private readonly Mock<IVerifyBookingAvailability> verifyBookingAvailability = new();
    private readonly Mock<ILogger<UpdateBooking>> logger = new();

    private readonly UpdateBookingDto dto = new()
    {
        Id = 1,
        StartDate = DateOnly.FromDateTime(DateTime.Now),
        EndDate = DateOnly.FromDateTime(DateTime.Now),
        RoomId = 1,
        CustomerId = 1
    };

    public UpdateBookingTests()
    {
        updateBooking = new(
            queryRepository.Object,
            commandRepository.Object,
            verifyBookingAvailability.Object,
            logger.Object
        );
        
        queryRepository.Setup(x => x.GetBookingById(It.IsAny<long>()))
            .ReturnsAsync(new Booking
            {
                StartDate = default,
                EndDate = default,
                RoomId = 0,
                CustomerId = 0
            });

        queryRepository.Setup(x => x.FindAsync(
                It.IsAny<Expression<Func<Booking, bool>>>()))
            .ReturnsAsync([]);
    }

    [Fact]
    public async Task UpdateBooking_DtoIsNull_ThrowsException()
    {
        var e = await Assert.ThrowsAsync<ArgumentNullException>(() => updateBooking.Handle(null!));

        Assert.Contains("dto", e.Message);
    }
    
    [Fact]
    public async Task UpdateBooking_IdIsZero_ThrowsException()
    {
        dto.Id = 0;
        
        var e = await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => updateBooking.Handle(dto));

        Assert.Contains("Id", e.Message);
    }
    
    [Fact]
    public async Task UpdateBooking_BookingIsNull_ThrowsException()
    {
        queryRepository.Setup(x => x.GetBookingById(It.IsAny<long>()))
            .ReturnsAsync((Booking?) null);
        
        var e = await Assert.ThrowsAsync<ArgumentException>(() => updateBooking.Handle(dto));

        Assert.Contains("does not exists", e.Message);
    }
    
    [Fact]
    public async Task UpdateBooking_BookingNotAvailable_ReturnsNull()
    {
        verifyBookingAvailability.Setup(x => x.Handle(
                It.IsAny<Booking>(), It.IsAny<IReadOnlyCollection<Booking>>()))
            .ReturnsAsync(false);

        var result = await updateBooking.Handle(dto);

        Assert.Null(result);
    }
    
    [Fact]
    public async Task UpdateBooking_BookingAvailable_ReturnsTheBooking()
    {
        verifyBookingAvailability.Setup(x => x.Handle(
                It.IsAny<Booking>(), It.IsAny<IReadOnlyCollection<Booking>>()))
            .ReturnsAsync(true);

        var result = await updateBooking.Handle(dto);

        Assert.NotNull(result);
    }
}