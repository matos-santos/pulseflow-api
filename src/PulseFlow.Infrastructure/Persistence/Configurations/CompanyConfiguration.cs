using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PulseFlow.Domain.Entities;

namespace PulseFlow.Infrastructure.Persistence.Configurations;

public class CompanyConfiguration : IEntityTypeConfiguration<Company>
{
    public void Configure(EntityTypeBuilder<Company> builder)
    {
        builder.ToTable("Companies");

        builder.HasKey(c => c.Id);

        builder
            .Property(c => c.Id)
            .ValueGeneratedNever()
            .IsRequired();

        builder
            .Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(255);

        builder
            .Property(c => c.Slug)
            .IsRequired()
            .HasMaxLength(255);

        builder
            .Property(c => c.IsActive)
            .IsRequired();

        builder
            .Property(c => c.CreatedAt)
            .IsRequired();

        builder
            .Property(c => c.CreatedBy)
            .HasMaxLength(255);

        builder
            .Property(c => c.UpdatedBy)
            .HasMaxLength(255);

        builder
            .Property(c => c.UpdatedAt)
            .IsRequired();

        builder
            .Property(c => c.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false);

        builder
            .Property(c => c.DeletedAt)
            .IsRequired(false);

        builder
            .Property(c => c.DeletedBy)
            .HasMaxLength(255)
            .IsRequired(false);

        builder
            .HasIndex(c => c.Name)
            .IsUnique()
            .HasDatabaseName("IX_Companies_Name");

        builder
            .HasIndex(c => c.Slug)
            .IsUnique()
            .HasDatabaseName("IX_Companies_Slug");

        builder
            .HasIndex(c => c.IsActive)
            .HasDatabaseName("IX_Companies_IsActive");

        builder
            .HasIndex(c => c.CreatedAt)
            .HasDatabaseName("IX_Companies_CreatedAt");

        builder
            .HasMany(c => c.Users)
            .WithOne(u => u.Company)
            .HasForeignKey(u => u.CompanyId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
