using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Model;

namespace Data.EntityConfiguration;

public class HotelConfiguration : IEntityTypeConfiguration<Hotel>
{
    public void Configure(EntityTypeBuilder<Hotel> builder)
    {
        builder.Property(x => x.Name).IsRequired().HasMaxLength(255);

        builder.HasOne(x => x.Room)
            .WithOne(x => x.Hotel)
            .HasForeignKey<Room>(x => x.HotelId)
            .IsRequired();
    }
}