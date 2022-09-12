using System.Linq.Expressions;
using BookingApi.Features.Booking;
using BookingApi.Features.Booking.Commands;
using Data;
using Data.Repository;

namespace BookingApi.UnitTests.Features.Booking.Commands;

[TestFixture]
public class BookRoomTests
{
#nullable disable
    private Mock<IUnitOfWork> unitOfWork;
    private Mock<IBookingRepository> bookingRepository;
    private Mock<IVerifyBookingAvailability> verifyBookingAvailability;
    private BookRoom bookRoom;
    private Model.Booking booking;
#nullable enable

    [SetUp]
    public void SetUp()
    {
        bookingRepository = new Mock<IBookingRepository>();
        verifyBookingAvailability = new Mock<IVerifyBookingAvailability>();

        bookingRepository.Setup(x =>
                x.Find(It.IsAny<Expression<Func<Model.Booking, bool>>>()))
            .Returns(new List<Model.Booking>());

        unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(x => x.Bookings).Returns(bookingRepository.Object);

        bookRoom = new BookRoom(unitOfWork.Object, verifyBookingAvailability.Object);
        booking = new Model.Booking();
    }

    [Test]
    public void Handle_BookingNotAvailable_ThrowBookingException()
    {
        verifyBookingAvailability.Setup(x =>
                x.Handle(It.IsAny<Model.Booking>(), It.IsAny<List<Model.Booking>>()))
            .Returns(false);

        Assert.That(() => bookRoom.Handle(booking),
            Throws.TypeOf(typeof(BookingException)));
    }

    [Test]
    public void Handle_WhenNoOverlapExist_AddTheBook()
    {
        verifyBookingAvailability.Setup(x =>
                x.Handle(It.IsAny<Model.Booking>(), It.IsAny<List<Model.Booking>>()))
            .Returns(true);

        bookRoom.Handle(booking);

        bookingRepository.Verify(x => x.Add(It.IsAny<Model.Booking>()));
    }
}