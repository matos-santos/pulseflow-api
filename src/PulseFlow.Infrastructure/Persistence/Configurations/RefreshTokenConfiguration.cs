using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PulseFlow.Domain.Entities;

namespace PulseFlow.Infrastructure.Persistence.Configurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {

        builder.ToTable("refresh_tokens");

        builder.HasKey(rt => rt.Id);

        builder
            .Property(rt => rt.Id)
            .ValueGeneratedNever()
            .IsRequired();

        builder
            .Property(rt => rt.UserId)
            .IsRequired();

        builder
            .Property(rt => rt.Token)
            .IsRequired()
            .HasMaxLength(500);

        builder
            .Property(rt => rt.ExpiresAt)
            .IsRequired();

        builder
            .HasOne(rt => rt.User)
            .WithMany(u => u.RefreshTokens)
            .HasForeignKey(rt => rt.UserId)
            .OnDelete(DeleteBehavior.Cascade);

    }
}
