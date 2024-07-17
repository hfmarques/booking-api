using Core.Domain.Entities;
using Core.Domain.Enums;
using Core.Tools;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Model.Enum;

namespace Data.EntityConfigurations;

public class BookingStatusConfiguration : IEntityTypeConfiguration<BookingStatus>
{
    public void Configure(EntityTypeBuilder<BookingStatus> builder)
    {
        builder
            .Property(e => e.Id)
            .HasConversion<int>();

        builder.Property(x => x.Name).HasMaxLength(100);
        builder.Property(x => x.Label).HasMaxLength(255);
        
        builder.HasData(
                Enum.GetValues(typeof(BookingStatusId))
                    .Cast<BookingStatusId>()
                    .Select(e => new BookingStatus
                    {
                        Id = e,
                        Name = e.ToString(),
                        Label = e.GetDescription()
                    })
            );
    }
}