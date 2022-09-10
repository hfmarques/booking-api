using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Model;

namespace Data;

public class EfContext : DbContext
{
    private readonly string connectionString;

    public EfContext(IConfiguration configuration)
    {
        connectionString = configuration.GetConnectionString("EfConnectionString");
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
        //modelBuilder.ApplyConfiguration(new CourseConfiguration());

        var room = new Room
        {
            Id = 1,
            Number = 101
        };

        modelBuilder.Entity<Room>().HasData(room);

        var hotel = new Hotel
        {
            Id = 1,
            Name = "Cancun Bay Resort",
            RoomId = 1
        };

        modelBuilder.Entity<Hotel>().HasData(hotel);
    }
}