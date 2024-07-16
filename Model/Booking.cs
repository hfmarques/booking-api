﻿using Model.Enum;

namespace Model;

public class Booking
{
    public long Id { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public BookingStatus Status { get; set; }
    public long RoomId { get; set; }
    public virtual Room Room { get; set; }
    public long CustomerId { get; set; }
    public virtual Customer Customer { get; set; }
}