using Asp.Versioning;
using PulseFlow.Api.Endpoints.V1;
using PulseFlow.Api.Extensions;
using PulseFlow.Api.Middlewares;
using PulseFlow.Infrastructure;
using Scalar.AspNetCore;
using Serilog;

namespace PulseFlow.Api;

public class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Configure Serilog
        builder.Host.UseSerilog((context, configuration) =>
        {
            configuration.ReadFrom.Configuration(context.Configuration);
        });

        // Add services to the container.

        // Add Infrastructure services (Database, Repositories, etc.)
        builder.Services.AddInfrastructure(builder.Configuration);

        builder.Services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ReportApiVersions = true;
            options.ApiVersionReader = new UrlSegmentApiVersionReader();
        });

        builder.Services.AddAuthorization();

        // Add Health Checks
        builder.Services.AddHealthCheckConfiguration();

        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        var app = builder.Build();

        // Apply database migrations in development
        if (app.Environment.IsDevelopment())
        {
            try
            {
                await app.Services.ApplyMigrationsAsync();
                app.Logger.LogInformation("Database migrations applied successfully");
            }
            catch (Exception ex)
            {
                app.Logger.LogError(ex, "An error occurred while applying migrations");
            }
        }

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.MapScalarApiReference(options =>
            {
                options.WithTitle("Pulse Flow API");
            });

            app.MapGet("/", () => Results.Redirect("/scalar/v1"))
                .ExcludeFromDescription();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        // Map Health Check endpoints
        app.MapHealthCheckEndpoints();

        var apiVersion = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1, 0))
            .ReportApiVersions()
            .Build();

        app.MapWeatherForecastsV1(apiVersion);

        app.UseSerilogRequestLogging();
        app.UseMiddleware<GlobalExceptionMiddleware>();

        app.Run();
    }
}