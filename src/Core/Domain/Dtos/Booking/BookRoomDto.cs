namespace Core.Domain.Dtos.Booking;

public class BookRoomDto
{
    public required DateOnly StartDate { get; set; }
    public required DateOnly EndDate { get; set; }
    public required long RoomId { get; set; }
    public required long CustomerId { get; set; }
}