using Core.Domain.Entities;
using Core.Domain.Enums;
using Core.Tools;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.EntityConfigurations;

public class RoomStatusConfiguration : IEntityTypeConfiguration<RoomStatus>
{

    public void Configure(EntityTypeBuilder<RoomStatus> builder)
    {
        builder
            .Property(e => e.Id)
            .HasConversion<int>();

        builder.Property(x => x.Name).HasMaxLength(100);
        builder.Property(x => x.Label).HasMaxLength(255);
        
        builder.HasData(
                Enum.GetValues(typeof(RoomStatusId))
                    .Cast<RoomStatusId>()
                    .Select(e => new RoomStatus
                    {
                        Id = e,
                        Name = e.ToString(),
                        Label = e.GetDescription()
                    })
            );
    }
}