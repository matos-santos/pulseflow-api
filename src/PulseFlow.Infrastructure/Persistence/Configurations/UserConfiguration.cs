using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PulseFlow.Domain.Entities;

namespace PulseFlow.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(u => u.Id);
        
        builder
            .Property(u => u.Id)
            .ValueGeneratedNever()
            .IsRequired();
        
        builder
            .Property(u => u.CompanyId)
            .IsRequired();

        builder
            .Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(255);
        
        builder
            .Property(u => u.FirstName)
            .IsRequired()
            .HasMaxLength(100);

        builder
            .Property(u => u.LastName)
            .IsRequired()
            .HasMaxLength(100);
        
        builder
            .Property(u => u.PhoneNumber)
            .HasMaxLength(20);
        
        builder
            .Property(u => u.PasswordHash)
            .IsRequired();

        builder
            .Property(u => u.IsActive)
            .IsRequired();

        builder
            .Property(u => u.Role)
            .IsRequired()
            .HasMaxLength(50);
        
        builder
            .Property(u => u.CreatedAt)
            .IsRequired();

        builder .Property(u => u.CreatedBy)
            .HasMaxLength(255);

        builder 
            .Property(u => u.UpdatedBy)
            .HasMaxLength(255);

        builder
            .Property(u => u.UpdatedAt)
            .IsRequired();

        builder
            .Property(u => u.LastLogin)
            .IsRequired(false);

        builder
            .Property(u => u.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false);

        builder
            .Property(u => u.DeletedAt)
            .IsRequired(false);

        builder
            .Property(u => u.DeletedBy)
            .HasMaxLength(255)
            .IsRequired(false);

        builder
            .HasIndex(u => u.Email)
            .IsUnique()
            .HasDatabaseName("IX_Users_Email");

        builder
            .HasIndex(u => u.IsActive)
            .HasDatabaseName("IX_Users_IsActive");

        builder
            .HasIndex(u => u.CreatedAt)
            .HasDatabaseName("IX_Users_CreatedAt");


        builder.HasOne(u => u.Company)
            .WithMany(c => c.Users)
            .HasForeignKey("CompanyId")
            .OnDelete(DeleteBehavior.Cascade);
    }
}
