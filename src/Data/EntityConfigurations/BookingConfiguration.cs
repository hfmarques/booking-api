using Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.EntityConfigurations;

public class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder
            .Property(e => e.StatusId)
            .HasConversion<int>();
        builder.HasOne(x => x.Status).WithMany().HasForeignKey(x => x.StatusId);

        builder.HasOne(x => x.Customer).WithMany().HasForeignKey(x => x.CustomerId);
        builder.HasOne(x => x.Room).WithMany().HasForeignKey(x => x.RoomId);
    }
}