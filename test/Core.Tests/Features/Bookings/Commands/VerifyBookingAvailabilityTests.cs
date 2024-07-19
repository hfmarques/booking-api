using Core.Domain.Entities;
using Core.Features.Booking.Commands;
using Core.Repositories;
using Microsoft.Extensions.Logging;
using Moq;

namespace Core.Tests.Features.Bookings.Commands;

public class VerifyBookingAvailabilityTests
{
    private readonly VerifyBookingAvailability verifyBookingAvailability;

    private readonly Mock<IQueryRepository<Domain.Entities.Room>> roomQueryRepository = new();
    private readonly Mock<ICustomerQueryRepository> customerQueryRepository = new();
    private readonly Mock<IVerifyBookingOverlapping> verifyBookingOverlapping = new();
    private readonly Mock<ILogger<VerifyBookingAvailability>> logger = new();

    private readonly Booking newBooking = new()
    {
        StartDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
        EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(2)),
        RoomId = 1,
        CustomerId = 1
    };
    public VerifyBookingAvailabilityTests()
    {
        verifyBookingAvailability = new(roomQueryRepository.Object,
            customerQueryRepository.Object,
            verifyBookingOverlapping.Object,
            logger.Object);

        roomQueryRepository.Setup(x =>
                x.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(new Domain.Entities.Room
            {
                Number = 1
            });

        customerQueryRepository.Setup(x =>
                x.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(new Domain.Entities.Customer
            {
                Name = "Test",
                Phone = "125465987"
            });
    }

    [Fact]
    public async Task VerifyBookingAvailability_BookingIsNull_ThrowsException()
    {
        var e = await Assert.ThrowsAsync<ArgumentNullException>(() => verifyBookingAvailability.Handle(null!, []));

        Assert.Contains("booking", e.Message);
    }

    [Fact]
    public async Task VerifyBookingAvailability_RoomIsNull_ThrowsException()
    {
        roomQueryRepository.Setup(x =>
                x.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync((Domain.Entities.Room) null!);

        var e = await Assert.ThrowsAsync<ArgumentNullException>(() => verifyBookingAvailability.Handle(newBooking, []));
        
        Assert.Contains("room", e.Message);
    }

    [Fact]
    public async Task VerifyBookingAvailability_CustomerIsNull_ThrowsException()
    {
        customerQueryRepository.Setup(x =>
                x.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync((Domain.Entities.Customer) null!);

        var e = await Assert.ThrowsAsync<ArgumentNullException>(() => verifyBookingAvailability.Handle(newBooking, []));
        
        Assert.Contains("customer", e.Message);
    }

    [Fact]
    public async Task VerifyBookingAvailability_EndDateLowerThanStartDate_ThrowsException()
    {
        newBooking.StartDate = DateOnly.FromDateTime(DateTime.Now);
        newBooking.EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-1));

        var e = await Assert.ThrowsAsync<ArgumentException>(() => 
            verifyBookingAvailability.Handle(newBooking, []));
        
        Assert.Contains("The startDate must be lower than endDate", e.Message);
    }

    [Fact]
    public async Task VerifyBookingAvailability_BookADayInThePast_ThrowsException()
    {
        newBooking.StartDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-1));
        newBooking.EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1));

        var e = await Assert.ThrowsAsync<ArgumentException>(() => 
            verifyBookingAvailability.Handle(newBooking, []));
        
        Assert.Contains("You cannot book a date in the pass", e.Message);
    }

    [Fact]
    public async Task VerifyBookingAvailability_StayLongerThan3Days_ThrowsException()
    {
        newBooking.StartDate = DateOnly.FromDateTime(DateTime.Now);
        newBooking.EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(4));

        var e = await Assert.ThrowsAsync<ArgumentException>(() => 
            verifyBookingAvailability.Handle(newBooking, []));
        
        Assert.Contains("The stay can't be longer than 3 days", e.Message);
    }

    [Fact]
    public async Task VerifyBookingAvailability_BookingReservedWithMoreThan30DaysAhead_ThrowsException()
    {
        newBooking.StartDate = DateOnly.FromDateTime(DateTime.Now.AddDays(31));
        newBooking.EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(33));

        var e = await Assert.ThrowsAsync<ArgumentException>(() => 
            verifyBookingAvailability.Handle(newBooking, []));
        
        Assert.Contains("The booking can't be reserved more than 30 days in advance", e.Message);
    }

    [Fact]
    public async Task VerifyBookingAvailability_WhenNoOverlappingExist_ReturnTrue()
    {
        verifyBookingOverlapping.Setup(x =>
                x.Handle(
                    It.IsAny<DateOnly>(),
                    It.IsAny<DateOnly>(),
                    It.IsAny<long>(),
                    It.IsAny<IReadOnlyCollection<Booking>>()))
            .Returns(true);

        var result = await verifyBookingAvailability.Handle(newBooking, []);

        Assert.True(result);
    }
}