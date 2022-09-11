using System.Linq.Expressions;
using BookingApi.Features.Booking;
using BookingApi.Features.Booking.Commands;
using Data;
using Data.Repository;
using Model;

namespace BookingApi.UnitTests.Features.Booking.Commands;

[TestFixture]
public class BookRoomTests
{
#nullable disable
    private Mock<IUnitOfWork> unitOfWork;
    private Mock<IRoomRepository> roomRepository;
    private Mock<IBookingRepository> bookingRepository;
    private Mock<ICustomerRepository> customerRepository;
    private BookRoom bookRoom;
    private Model.Booking booking;
#nullable enable

    [SetUp]
    public void SetUp()
    {
        roomRepository = new Mock<IRoomRepository>();
        bookingRepository = new Mock<IBookingRepository>();
        customerRepository = new Mock<ICustomerRepository>();

        unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(x => x.Rooms).Returns(roomRepository.Object);
        unitOfWork.Setup(x => x.Customers).Returns(customerRepository.Object);
        unitOfWork.Setup(x => x.Bookings).Returns(bookingRepository.Object);

        roomRepository.Setup(x =>
                x.Get(It.IsAny<long>()))
            .Returns(new Room());

        customerRepository.Setup(x =>
                x.Get(It.IsAny<long>()))
            .Returns(new Model.Customer());

        bookRoom = new BookRoom(unitOfWork.Object);
        booking = new Model.Booking();
    }

    [Test]
    public void Handle_BookingIsNull_ThrowsArgumentNullException()
    {
        Assert.That(() => bookRoom.Handle(null!), Throws.ArgumentNullException);
    }

    [Test]
    public void Handle_RoomIsNull_ThrowsArgumentException()
    {
        roomRepository.Setup(x =>
                x.Get(It.IsAny<long>()))
            .Returns((Room) null!);

        Assert.That(() => bookRoom.Handle(booking), Throws.ArgumentException);
    }

    [Test]
    public void Handle_CustomerIsNull_ThrowsArgumentException()
    {
        customerRepository.Setup(x =>
                x.Get(It.IsAny<long>()))
            .Returns((Model.Customer) null!);

        Assert.That(() => bookRoom.Handle(booking), Throws.ArgumentException);
    }

    [Test]
    public void Handle_EndDateLowerThanStartDate_ThrowsBookingException()
    {
        booking.StartDate = DateTime.Now;
        booking.EndDate = DateTime.Now.AddDays(-1);

        Assert.That(() => bookRoom.Handle(booking),
            Throws.TypeOf(typeof(BookingException)));
    }

    [Test]
    public void Handle_BookADayInThePast_ThrowsBookingException()
    {
        booking.StartDate = DateTime.Now.AddDays(-1);
        booking.EndDate = DateTime.Now.AddDays(1);

        Assert.That(() => bookRoom.Handle(booking),
            Throws.TypeOf(typeof(BookingException)));
    }

    [Test]
    public void Handle_StayLongerThan3Days_ThrowsBookingException()
    {
        booking.StartDate = DateTime.Now;
        booking.EndDate = DateTime.Now.AddDays(5);

        Assert.That(() => bookRoom.Handle(booking),
            Throws.TypeOf(typeof(BookingException)));
    }

    [Test]
    public void Handle_BookingReservedWithMoreThan30DaysAhead_ThrowsBookingException()
    {
        booking.StartDate = DateTime.Now.AddDays(31);
        booking.StartDate = DateTime.Now.AddDays(32);

        Assert.That(() => bookRoom.Handle(booking),
            Throws.TypeOf(typeof(BookingException)));
    }

    [Test]
    [TestCase(1, 3, 2, 4)]
    [TestCase(3, 4, 2, 4)]
    [TestCase(3, 5, 2, 4)]
    [TestCase(3, 5, 4, 4)]
    public void Handle_WhenOverlappingExist_ThrowBookingException(
        int bookStart,
        int bookEnd,
        int existBookStart,
        int existBookEnd)
    {
        booking.StartDate = DateTime.Now.AddDays(bookStart);
        booking.EndDate = DateTime.Now.AddDays(bookEnd);

        var overlappingBooking = new Model.Booking
        {
            StartDate = DateTime.Now.AddDays(existBookStart),
            EndDate = DateTime.Now.AddDays(existBookEnd)
        };

        bookingRepository.Setup(x =>
                x.Find(It.IsAny<Expression<Func<Model.Booking, bool>>>()))
            .Returns(new List<Model.Booking> {overlappingBooking});

        Assert.That(() => bookRoom.Handle(booking),
            Throws.TypeOf(typeof(BookingException)));
    }

    [Test]
    public void Handle_WhenNoOverlapExist_ThrowsNothing()
    {
        booking.StartDate = DateTime.Now.AddDays(1);
        booking.EndDate = DateTime.Now.AddDays(2);

        bookingRepository.Setup(x =>
                x.Find(It.IsAny<Expression<Func<Model.Booking, bool>>>()))
            .Returns(new List<Model.Booking>());

        Assert.That(() => bookRoom.Handle(booking),
            Throws.Nothing);
    }
}