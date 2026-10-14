using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OmniReserve.Domain.Entities;

namespace OmniReserve.Infrastructure.Persistence.Configurations;

public class RoomConfiguration : IEntityTypeConfiguration<Room>
{
    public void Configure(EntityTypeBuilder<Room> builder)
    {
        builder.ToTable("Rooms");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.RoomNumber)
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(x => x.PricePerNight)
            .HasPrecision(18, 2);

        builder.Property(x => x.Type)
            .HasConversion<string>();
    }
}