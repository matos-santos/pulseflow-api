using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PulseFlow.Domain.Repositories;
using PulseFlow.Infrastructure.Persistence;
using PulseFlow.Infrastructure.Persistence.Interceptors;
using PulseFlow.Infrastructure.Repositories;

namespace PulseFlow.Infrastructure;

/// <summary>
/// Extension methods for configuring Infrastructure services
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Adds Infrastructure services to the service collection
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <param name="configuration">The configuration</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Register interceptors
        services.AddScoped<AuditableEntityInterceptor>();

        // Register DbContext
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(connectionString, npgsqlOptions =>
            {
                npgsqlOptions.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
                npgsqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(10),
                    errorCodesToAdd: null);
            });

            // Enable sensitive data logging in development
            if (configuration["Logging:EnableSensitiveDataLogging"] == "true")
            {
                options.EnableSensitiveDataLogging();
            }

            // Enable detailed errors in development
            if (configuration["Logging:EnableDetailedErrors"] == "true")
            {
                options.EnableDetailedErrors();
            }
        });

        // Register repositories
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped(typeof(IRepository<,>), typeof(BaseRepository<,>));

        // Register specific repositories
        services.AddScoped<ISampleRepository, SampleRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }

    /// <summary>
    /// Applies pending migrations to the database
    /// </summary>
    /// <param name="services">The service provider</param>
    /// <returns>A task representing the asynchronous operation</returns>
    public static async Task ApplyMigrationsAsync(this IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        if ((await context.Database.GetPendingMigrationsAsync()).Any())
        {
            await context.Database.MigrateAsync();
        }
    }
}
