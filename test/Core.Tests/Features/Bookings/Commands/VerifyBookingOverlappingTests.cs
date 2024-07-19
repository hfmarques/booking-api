using Core.Domain.Entities;
using Core.Domain.Enums;
using Core.Features.Booking.Commands;

namespace Core.Tests.Features.Bookings.Commands;

public class VerifyBookingOverlappingTests
{
    private readonly VerifyBookingOverlapping verifyBookingOverlapping = new ();
    private readonly List<Booking> existedBookings = [];

    [Theory]
    [InlineData(1, 4, 3, 6)]
    [InlineData(3, 6, 3, 6)]
    [InlineData(4, 5, 3, 6)]
    [InlineData(5, 7, 3, 6)]
    [InlineData(1, 7, 3, 6)]
    public void VerifyBookingOverlapping_WhenOverlappingExist_ReturnFalse(
        int bookStart,
        int bookEnd,
        int existBookStart,
        int existBookEnd)
    {
        var startDate = DateOnly.FromDateTime(DateTime.Now).AddDays(bookStart);
        var endDate = DateOnly.FromDateTime(DateTime.Now).AddDays(bookEnd);
        const int roomId = 1;

        existedBookings.Add(new()
        {
            StartDate = DateOnly.FromDateTime(DateTime.Now)
                .AddDays(existBookStart),
            EndDate = DateOnly.FromDateTime(DateTime.Now)
                .AddDays(existBookEnd),
            StatusId = BookingStatusId.Confirmed,
            RoomId = roomId,
            CustomerId = 1
        });

        var result = verifyBookingOverlapping.Handle(startDate, endDate, roomId, existedBookings);

        Assert.False(result);
    }
    
    [Theory]
    [InlineData(1, 2, 3, 6)]
    [InlineData(2, 2, 3, 6)]
    [InlineData(7, 7, 3, 6)]
    [InlineData(7, 8, 3, 6)]
    public void VerifyBookingOverlapping_WhenOverlappingDoNotExist_ReturnTrue(
        int bookStart,
        int bookEnd,
        int existBookStart,
        int existBookEnd)
    {
        var startDate = DateOnly.FromDateTime(DateTime.Now).AddDays(bookStart);
        var endDate = DateOnly.FromDateTime(DateTime.Now).AddDays(bookEnd);
        const int roomId = 1;

        existedBookings.Add(new()
        {
            StartDate = 
                DateOnly.FromDateTime(DateTime.Now).AddDays(existBookStart),
            EndDate = 
                DateOnly.FromDateTime(DateTime.Now).AddDays(existBookEnd),
            StatusId = BookingStatusId.Confirmed,
            RoomId = roomId,
            CustomerId = 1
        });

        var result = verifyBookingOverlapping.Handle(startDate, endDate, roomId, existedBookings);

        Assert.True(result);
    }
}