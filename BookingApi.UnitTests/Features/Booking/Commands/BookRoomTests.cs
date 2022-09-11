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
            .Returns(new Customer());

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
            .Returns((Customer) null!);

        Assert.That(() => bookRoom.Handle(booking), Throws.ArgumentException);
    }
}