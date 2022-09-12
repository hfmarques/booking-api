using BookingApi.Features.Booking;
using BookingApi.Features.Booking.Commands;
using Data;
using Data.Repository;
using Model;

namespace BookingApi.UnitTests.Features.Booking.Commands;

[TestFixture]
public class VerifyBookingAvailabilityTests
{
#nullable disable
    private Mock<IUnitOfWork> unitOfWork;
    private Mock<IRoomRepository> roomRepository;
    private Mock<ICustomerRepository> customerRepository;
    private Mock<IVerifyBookingOverlapping> verifyBookingOverlapping;
    private VerifyBookingAvailability verifyBookingAvailability;
    private Model.Booking newBooking;
    private List<Model.Booking> existedBookings;
#nullable enable

    [SetUp]
    public void SetUp()
    {
        roomRepository = new Mock<IRoomRepository>();
        customerRepository = new Mock<ICustomerRepository>();
        verifyBookingOverlapping = new Mock<IVerifyBookingOverlapping>();

        unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(x => x.Rooms).Returns(roomRepository.Object);
        unitOfWork.Setup(x => x.Customers).Returns(customerRepository.Object);

        roomRepository.Setup(x =>
                x.Get(It.IsAny<long>()))
            .Returns(new Room());

        customerRepository.Setup(x =>
                x.Get(It.IsAny<long>()))
            .Returns(new Model.Customer());

        verifyBookingAvailability = new VerifyBookingAvailability(unitOfWork.Object, verifyBookingOverlapping.Object);
        newBooking = new Model.Booking
        {
            StartDate = DateTime.Now.AddDays(1),
            EndDate = DateTime.Now.AddDays(2),
            RoomId = 1,
            CustomerId = 1
        };
        existedBookings = new List<Model.Booking>();
    }

    [Test]
    public void Handle_BookingIsNull_ThrowsArgumentNullException()
    {
        Assert.That(() =>
                verifyBookingAvailability.Handle(null!, existedBookings),
            Throws.ArgumentNullException);
    }

    [Test]
    public void Handle_RoomIsNull_ThrowsArgumentException()
    {
        roomRepository.Setup(x =>
                x.Get(It.IsAny<long>()))
            .Returns((Room) null!);

        Assert.That(() => verifyBookingAvailability.Handle(
                newBooking,
                existedBookings),
            Throws.ArgumentException);
    }

    [Test]
    public void Handle_CustomerIsNull_ThrowsArgumentException()
    {
        customerRepository.Setup(x =>
                x.Get(It.IsAny<long>()))
            .Returns((Model.Customer) null!);

        Assert.That(() => verifyBookingAvailability.Handle(
                newBooking,
                existedBookings),
            Throws.ArgumentException);
    }

    [Test]
    public void Handle_EndDateLowerThanStartDate_ThrowsBookingException()
    {
        newBooking.StartDate = DateTime.Now;
        newBooking.EndDate = DateTime.Now.AddDays(-1);

        Assert.That(() => verifyBookingAvailability.Handle(newBooking,
                existedBookings),
            Throws.TypeOf(typeof(BookingException)));
    }

    [Test]
    public void Handle_BookADayInThePast_ThrowsBookingException()
    {
        newBooking.StartDate = DateTime.Now.AddDays(-1);
        newBooking.EndDate = DateTime.Now.AddDays(1);

        Assert.That(() => verifyBookingAvailability.Handle(newBooking,
                existedBookings),
            Throws.TypeOf(typeof(BookingException)));
    }

    [Test]
    public void Handle_StayLongerThan3Days_ThrowsBookingException()
    {
        newBooking.StartDate = DateTime.Now;
        newBooking.EndDate = DateTime.Now.AddDays(3);

        Assert.That(() => verifyBookingAvailability.Handle(
                newBooking,
                existedBookings),
            Throws.TypeOf(typeof(BookingException)));
    }

    [Test]
    public void Handle_BookingReservedWithMoreThan30DaysAhead_ThrowsBookingException()
    {
        newBooking.StartDate = DateTime.Now.AddDays(30);
        newBooking.EndDate = DateTime.Now.AddDays(32);

        Assert.That(() => verifyBookingAvailability.Handle(
                newBooking,
                existedBookings),
            Throws.TypeOf(typeof(BookingException)));
    }

    [Test]
    public void Handle_WhenOverlappingExist_ReturnFalse()
    {
        verifyBookingOverlapping.Setup(x =>
                x.Handle(
                    It.IsAny<DateTime>(),
                    It.IsAny<DateTime>(),
                    It.IsAny<long>(),
                    It.IsAny<List<Model.Booking>>()))
            .Returns(false);

        var result = verifyBookingAvailability.Handle(newBooking, existedBookings);

        Assert.That(result, Is.EqualTo(false));
    }

    [Test]
    public void Handle_WhenNoOverlapExist_ReturnTrue()
    {
        verifyBookingOverlapping.Setup(x =>
                x.Handle(
                    It.IsAny<DateTime>(),
                    It.IsAny<DateTime>(),
                    It.IsAny<long>(),
                    It.IsAny<List<Model.Booking>>()))
            .Returns(true);

        var result = verifyBookingAvailability.Handle(newBooking, existedBookings);
        Assert.That(result, Is.EqualTo(true));
    }
}