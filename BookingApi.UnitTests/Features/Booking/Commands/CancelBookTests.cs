using BookingApi.Features.Booking.Commands;
using Data;
using Data.Repository;
using Model.Enum;

namespace BookingApi.UnitTests.Features.Booking.Commands;

[TestFixture]
public class CancelBookTests
{
#nullable disable
    private Mock<IUnitOfWork> unitOfWork;
    private Mock<IBookingRepository> bookingRepository;
    private CancelBook cancelBook;
#nullable enable

    [SetUp]
    public void SetUp()
    {
        bookingRepository = new Mock<IBookingRepository>();

        unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(x => x.Bookings).Returns(bookingRepository.Object);

        cancelBook = new CancelBook(unitOfWork.Object);
    }

    [Test]
    public void Handle_IdIsZero_ThrowsArgumentException()
    {
        Assert.That(() => cancelBook.Handle(0), Throws.ArgumentException);
    }

    [Test]
    public void Handle_BookDoesNotExists_ThrowsArgumentException()
    {
        bookingRepository.Setup(x =>
                x.Get(It.IsAny<long>()))
            .Returns((Model.Booking) null!);

        Assert.That(() => cancelBook.Handle(1), Throws.ArgumentException);
    }

    [Test]
    public void Handle_WhenCalled_ChangeBookStatus()
    {
        bookingRepository.Setup(x =>
                x.Get(It.IsAny<long>()))
            .Returns(new Model.Booking());

        var result = cancelBook.Handle(1);
        
        Assert.That(result.Status, Is.EqualTo(BookingStatus.Cancelled));
    }
}