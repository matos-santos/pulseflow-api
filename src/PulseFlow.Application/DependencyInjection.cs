using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PulseFlow.Application.Common.Behaviors;
using PulseFlow.Application.Common.Interfaces;
using PulseFlow.Application.Common.Services;

namespace PulseFlow.Application;

public static class DependencyInjection
{

    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(typeof(DependencyInjection).Assembly);
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

        services.AddScoped<IJwtService, JwtService>();

        return services;
    }
}
