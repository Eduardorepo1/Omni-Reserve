using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OmniReserve.Domain.Entities;
using OmniReserve.Domain.ValueObjects;

namespace OmniReserve.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Email)
            .HasConversion(
                emailObj => emailObj.Value,
                emailStr => new EmailAddress(emailStr))
            .IsRequired()
            .HasColumnName("Email")
            .HasMaxLength(255);

        builder.HasIndex(x => x.Email)
            .IsUnique();

        builder.Property(x => x.FirstName)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(x => x.LastName)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(x => x.PasswordHash)
            .IsRequired();

        builder.Property(x => x.Role)
            .IsRequired()
            .HasMaxLength(50)
            .HasDefaultValue("Client");
    }
}