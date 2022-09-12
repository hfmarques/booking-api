using BookingApi.Features.Booking.Commands;
using Model.Enum;

namespace BookingApi.UnitTests.Features.Booking.Commands;

[TestFixture]
public class VerifyBookingOverlappingTests
{
    private VerifyBookingOverlapping verifyBookingOverlapping;
    private List<Model.Booking> existedBookings;

    [SetUp]
    public void SetUp()
    {
        verifyBookingOverlapping = new VerifyBookingOverlapping();
        existedBookings = new List<Model.Booking>();
    }

    [Test]
    [TestCase(1, 2, 2, 4)]
    [TestCase(1, 3, 2, 4)]
    [TestCase(2, 3, 2, 4)]
    [TestCase(2, 4, 2, 4)]
    [TestCase(3, 4, 2, 4)]
    [TestCase(3, 5, 2, 4)]
    [TestCase(4, 5, 2, 4)]
    [TestCase(3, 3, 2, 4)]
    [TestCase(1, 3, 2, 2)]
    public void Handle_WhenOverlappingExist_ReturnFalse(
        int bookStart,
        int bookEnd,
        int existBookStart,
        int existBookEnd)
    {
        var startDate = DateTime.Now.AddDays(bookStart);
        var endDate = DateTime.Now.AddDays(bookEnd);
        const int roomId = 1;

        existedBookings.Add(new Model.Booking
        {
            StartDate = DateTime.Now.AddDays(existBookStart),
            EndDate = DateTime.Now.AddDays(existBookEnd),
            Status = BookingStatus.Confirmed,
            RoomId = roomId
        });

        var result = verifyBookingOverlapping.Handle(startDate, endDate, roomId, existedBookings);

        Assert.That(result, Is.EqualTo(false));
    }
    
    [Test]
    public void Handle_WhenExistingBookingsCountIsZero_ReturnTrue()
    {
        var startDate = DateTime.Now.AddDays(1);
        var endDate = DateTime.Now.AddDays(2);
        const int roomId = 1;

        var result = verifyBookingOverlapping.Handle(startDate, endDate, roomId, existedBookings);

        Assert.That(result, Is.EqualTo(true));
    }

    [Test]
    public void Handle_WhenNoOverlapExist_ReturnTrue()
    {
        var startDate = DateTime.Now.AddDays(1);
        var endDate = DateTime.Now.AddDays(2);
        const int roomId = 1;


        existedBookings.Add(new Model.Booking
        {
            StartDate = DateTime.Now.AddDays(4),
            EndDate = DateTime.Now.AddDays(5),
            Status = BookingStatus.Confirmed,
            RoomId = roomId
        });

        var result = verifyBookingOverlapping.Handle(startDate, endDate, roomId, existedBookings);
        Assert.That(result, Is.EqualTo(true));
    }
}