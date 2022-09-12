using BookingApi.Features.Booking;
using BookingApi.Features.Booking.Commands;
using Data;
using Data.Repository;

namespace BookingApi.UnitTests.Features.Booking.Commands;

[TestFixture]
public class UpdateBookingTests
{
#nullable disable
    private Mock<IUnitOfWork> unitOfWork;
    private Mock<IBookingRepository> bookingRepository;
    private Mock<IVerifyBookingAvailability> verifyBookingAvailability;
    private UpdateBooking updateBooking;
#nullable enable

    [SetUp]
    public void SetUp()
    {
        bookingRepository = new Mock<IBookingRepository>();
        verifyBookingAvailability = new Mock<IVerifyBookingAvailability>();

        bookingRepository.Setup(x => x.Get(It.IsAny<long>())).Returns(new Model.Booking());

        unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(x => x.Bookings).Returns(bookingRepository.Object);

        updateBooking = new UpdateBooking(unitOfWork.Object, verifyBookingAvailability.Object);
    }

    [Test]
    public void Handle_BookingIsNull_ThrowsArgumentNullException()
    {
        Assert.That(() => updateBooking.Handle(null!), Throws.ArgumentNullException);
    }

    [Test]
    public void Handle_IdIsZero_ThrowsArgumentException()
    {
        Assert.That(() => updateBooking.Handle(new Model.Booking {Id = 0}), Throws.ArgumentException);
    }

    [Test]
    public void Handle_BookDoesNotExists_ThrowsArgumentException()
    {
        bookingRepository.Setup(x =>
                x.Get(It.IsAny<long>()))
            .Returns((Model.Booking) null!);

        Assert.That(() => updateBooking.Handle(new Model.Booking()), Throws.ArgumentException);
    }

    [Test]
    public void Handle_BookingNotAvailable_ThrowBookingException()
    {
        verifyBookingAvailability.Setup(x =>
                x.Handle(It.IsAny<Model.Booking>(), It.IsAny<List<Model.Booking>>()))
            .Returns(false);

        Assert.That(() => updateBooking.Handle(new Model.Booking {Id = 1}),
            Throws.TypeOf(typeof(BookingException)));
    }

    [Test]
    public void Handle_WhenNoOverlapExist_UpdateTheBook()
    {
        verifyBookingAvailability.Setup(x =>
                x.Handle(It.IsAny<Model.Booking>(), It.IsAny<List<Model.Booking>>()))
            .Returns(true);
        
        updateBooking.Handle(new Model.Booking {Id = 1});

        unitOfWork.Verify(x => x.SaveChanges());
    }
}