﻿namespace Model;

#nullable disable
public class Room
{
    public long Id { get; set; }
    public int Number { get; set; }
    public long HotelId { get; set; }
    public virtual Hotel Hotel { get; set; }
    public virtual ICollection<Booking> Bookings { get; set; }
}