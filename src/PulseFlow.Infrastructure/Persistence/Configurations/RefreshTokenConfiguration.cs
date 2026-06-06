using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PulseFlow.Domain.Entities;

namespace PulseFlow.Infrastructure.Persistence.Configurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {

        builder.ToTable("RefreshTokens");

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
            .Property(rt => rt.IsRevoked)
            .IsRequired()
            .HasDefaultValue(false);

        builder
            .Property(rt => rt.RevokedReason)
            .IsRequired(false)
            .HasMaxLength(200);

        builder
            .Property(rt => rt.RevokedAt)
            .IsRequired(false);

        builder
            .Property(rt => rt.ReplacedByToken)
            .HasMaxLength(500)
            .IsRequired(false);

        builder
            .Property(rt => rt.AccessTokenJti)
            .HasMaxLength(100)
            .IsRequired(false);

        builder
            .Property(rt => rt.ExpiresAt)
            .IsRequired();

        builder
            .HasOne(rt => rt.User)
            .WithMany(u => u.RefreshTokens)
            .HasForeignKey(rt => rt.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(rt => rt.Token)
            .HasDatabaseName("IX_RefreshTokens_Token")
            .IsUnique();

        builder.HasIndex(rt => rt.AccessTokenJti)
            .HasDatabaseName("IX_RefreshTokens_AccessTokenJti")
            .IsUnique();


    }
}
