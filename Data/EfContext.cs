using Data.EntityConfiguration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Model;

namespace Data;

public class EfContext : DbContext
{
    private readonly string connectionString;

    public EfContext(IConfiguration configuration)
    {
        connectionString = configuration.GetConnectionString("Ef");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(connectionString);
        }
    }

#nullable disable
    public virtual DbSet<Hotel> Hotels { get; set; }
    public virtual DbSet<Room> Rooms { get; set; }
    public virtual DbSet<Booking> Bookings { get; set; }
    public virtual DbSet<Customer> Customers { get; set; }
#nullable enable

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new HotelConfiguration());
        modelBuilder.ApplyConfiguration(new RoomConfiguration());
        modelBuilder.ApplyConfiguration(new BookingConfiguration());
        modelBuilder.ApplyConfiguration(new CustomerConfiguration());

        var hotel = new Hotel
        {
            Id = 1,
            Name = "Cancun Bay Resort"
        };

        modelBuilder.Entity<Hotel>().HasData(hotel);

        var room = new Room
        {
            Id = 1,
            Number = 101,
            HotelId = 1
        };

        modelBuilder.Entity<Room>().HasData(room);
    }
}