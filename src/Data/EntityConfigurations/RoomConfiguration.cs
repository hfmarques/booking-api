using Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.EntityConfigurations;

public class RoomConfiguration : IEntityTypeConfiguration<Room>
{
    public void Configure(EntityTypeBuilder<Room> builder)
    {
        builder
            .Property(e => e.StatusId)
            .HasConversion<int>();
        builder.HasOne(x => x.Status).WithMany().HasForeignKey(x => x.StatusId);
    }
}