using System.Reflection;
using Microsoft.EntityFrameworkCore;
using PulseFlow.Domain.Entities;
using PulseFlow.Infrastructure.Persistence.Interceptors;

namespace PulseFlow.Infrastructure.Persistence;

/// <summary>
/// Application database context
/// </summary>
public class ApplicationDbContext : DbContext
{
    private readonly AuditableEntityInterceptor _auditableEntityInterceptor;

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        AuditableEntityInterceptor auditableEntityInterceptor) 
        : base(options)
    {
        _auditableEntityInterceptor = auditableEntityInterceptor;
    }

    // DbSets
    public DbSet<SampleEntity> Samples => Set<SampleEntity>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Company> Companies => Set<Company>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(_auditableEntityInterceptor);

        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Apply all configurations from this assembly
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        // Apply global query filters for soft delete
        modelBuilder.Entity<SampleEntity>()
            .HasQueryFilter(e => !EF.Property<bool>(e, "IsDeleted"));
        
        modelBuilder.Entity<User>()
            .HasQueryFilter(e => !EF.Property<bool>(e, "IsDeleted"));

        base.OnModelCreating(modelBuilder);
    }
}
