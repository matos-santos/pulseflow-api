using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace PulseFlow.Api.Extensions;

public static class HealthCheckExtensions
{
    public static IServiceCollection AddHealthCheckConfiguration(this IServiceCollection services)
    {
        services.AddHealthChecks();
        // Adicione aqui novos health checks conforme o projeto evolui:
        // .AddSqlServer(connectionString, tags: new[] { "ready", "db" })
        // .AddRedis(redisConnectionString, tags: new[] { "ready", "cache" })
        // .AddRabbitMQ(rabbitConnectionString, tags: new[] { "ready", "messaging" })

        return services;
    }

    public static WebApplication MapHealthCheckEndpoints(this WebApplication app)
    {
        // Endpoint principal - retorna status de todos os health checks
        app.MapHealthChecks("/health", new HealthCheckOptions
        {
            Predicate = _ => true,
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });

        // Endpoint de readiness - verifica se a aplicação está pronta para receber tráfego
        // Útil para Kubernetes readiness probes
        app.MapHealthChecks("/health/ready", new HealthCheckOptions
        {
            Predicate = check => check.Tags.Contains("ready"),
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });

        // Endpoint de liveness - verifica se a aplicação está viva
        // Útil para Kubernetes liveness probes
        app.MapHealthChecks("/health/live", new HealthCheckOptions
        {
            Predicate = _ => false, // Não executa nenhum check específico, apenas verifica se a app responde
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });

        return app;
    }
}
