using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Model;

namespace Data.EntityConfiguration;

public class HotelConfiguration : IEntityTypeConfiguration<Hotel>
{
    public void Configure(EntityTypeBuilder<Hotel> builder)
    {
        builder.Property(x => x.Name).IsRequired().HasMaxLength(255);

        builder.HasMany(x => x.Room)
            .WithOne(x => x.Hotel)
            .HasForeignKey(x => x.HotelId)
            .IsRequired();
    }
}