using Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.EntityConfigurations;

public class HotelConfiguration : IEntityTypeConfiguration<Hotel>
{
    public void Configure(EntityTypeBuilder<Hotel> builder)
    {
        builder.Property(x => x.Name).HasMaxLength(255);

        builder.HasMany(x => x.Rooms)
            .WithOne()
            .HasForeignKey(x => x.HotelId)
            .IsRequired();
    }
}