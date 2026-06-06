using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PulseFlow.Domain.Entities;

namespace PulseFlow.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for SampleEntity
/// </summary>
public class SampleEntityConfiguration : IEntityTypeConfiguration<SampleEntity>
{
    public void Configure(EntityTypeBuilder<SampleEntity> builder)
    {
        builder.ToTable("Samples");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .ValueGeneratedNever()
            .IsRequired();

        builder.Property(e => e.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(e => e.Description)
            .HasMaxLength(1000);

        builder.Property(e => e.IsActive)
            .IsRequired();

        // Audit fields
        builder.Property(e => e.CreatedAt)
            .IsRequired();

        builder.Property(e => e.CreatedBy)
            .HasMaxLength(100);

        builder.Property(e => e.UpdatedAt);

        builder.Property(e => e.UpdatedBy)
            .HasMaxLength(100);

        builder.Property(e => e.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(e => e.DeletedAt)
            .IsRequired(false);

        builder.Property(e => e.DeletedBy)
            .HasMaxLength(100)
            .IsRequired(false);

        // Indexes
        builder.HasIndex(e => e.Name)
            .HasDatabaseName("IX_Samples_Name");

        builder.HasIndex(e => e.IsActive)
            .HasDatabaseName("IX_Samples_IsActive");

        builder.HasIndex(e => e.CreatedAt)
            .HasDatabaseName("IX_Samples_CreatedAt");
    }
}
