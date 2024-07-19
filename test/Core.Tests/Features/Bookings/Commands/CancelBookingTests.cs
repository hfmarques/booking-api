using Core.Domain.Entities;
using Core.Features.Booking.Commands;
using Core.Repositories;
using Microsoft.Extensions.Logging;
using Moq;

namespace Core.Tests.Features.Bookings.Commands;

public class CancelBookingTests
{
    private readonly CancelBooking cancelBooking;

    private readonly Mock<IBookingQueryRepository> queryRepository = new();
    private readonly Mock<ICommandRepository<Domain.Entities.Booking>> commandRepository = new();
    private readonly Mock<ILogger<CancelBooking>> logger = new();

    public CancelBookingTests()
    {
        cancelBooking = new(
            queryRepository.Object,
            commandRepository.Object,
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
    }
    
    [Fact]
    public async Task CancelBooking_IdIsZero_ThrowsException()
    {
        const int id = 0;
        
        var e = await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => cancelBooking.Handle(id));

        Assert.Contains("id", e.Message);
    }
    
    [Fact]
    public async Task CancelBooking_BookingIsNull_ThrowsException()
    {
        queryRepository.Setup(x => x.GetBookingById(It.IsAny<long>()))
            .ReturnsAsync((Booking?) null);
        
        var e = await Assert.ThrowsAsync<ArgumentException>(() => cancelBooking.Handle(1));

        Assert.Contains("does not exists", e.Message);
    }
}