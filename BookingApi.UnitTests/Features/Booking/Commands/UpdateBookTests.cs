using BookingApi.Features.Booking.Commands;
using Data;
using Data.Repository;

namespace BookingApi.UnitTests.Features.Booking.Commands;

[TestFixture]
public class UpdateBookTests
{
#nullable disable
    private Mock<IUnitOfWork> unitOfWork;
    private Mock<IBookingRepository> bookingRepository;
    private UpdateBook updateBook;
#nullable enable

    [SetUp]
    public void SetUp()
    {
        bookingRepository = new Mock<IBookingRepository>();

        unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(x => x.Bookings).Returns(bookingRepository.Object);

        updateBook = new UpdateBook(unitOfWork.Object);
    }

    [Test]
    public void Handle_BookingIsNull_ThrowsArgumentNullException()
    {
        Assert.That(() => updateBook.Handle(null!), Throws.ArgumentNullException);
    }

    [Test]
    public void Handle_IdIsZero_ThrowsArgumentException()
    {
        Assert.That(() => updateBook.Handle(new Model.Booking {Id = 0}), Throws.ArgumentException);
    }

    [Test]
    public void Handle_BookDoesNotExists_ThrowsArgumentException()
    {
        bookingRepository.Setup(x =>
                x.Get(It.IsAny<long>()))
            .Returns((Model.Booking) null!);

        Assert.That(() => updateBook.Handle(new Model.Booking()), Throws.ArgumentException);
    }

    [Test]
    public void Handle_WhenCalled_UpdateBook()
    {
        bookingRepository.Setup(x =>
                x.Get(It.IsAny<long>()))
            .Returns(new Model.Booking());

        Assert.That(() => updateBook.Handle(new Model.Booking{Id = 1}), Throws.Nothing);
    }
}