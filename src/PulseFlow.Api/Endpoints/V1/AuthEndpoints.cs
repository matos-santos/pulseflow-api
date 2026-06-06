using Asp.Versioning.Builder;
using MediatR;
using PulseFlow.Application.UseCase.Auth.Login;

namespace PulseFlow.Api.Endpoints.V1;

public static class AuthEndpoints
{
    public static IEndpointRouteBuilder MapAuthEndpointsV1(this IEndpointRouteBuilder app, ApiVersionSet version)
    {
        var group = app.MapGroup("/api/v{version:apiVersion}/auth")
            .WithApiVersionSet(version)
            .WithTags("Authentication")
            .WithOpenApi();

        group.MapPost("/login", Login)
            .MapToApiVersion(1.0)
            .WithName("Login")
            .WithSummary("Realiza o login de um usuário e retorna suas credenciais")
            .WithDescription("Endpoint para autenticação de usuários. Retorna um token JWT em caso de sucesso.")
            .Produces<LoginOutput>(StatusCodes.Status200OK)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .AllowAnonymous();
        return app;
    }

    private static async Task<IResult> Login(
        LoginInput loginInput,
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(loginInput, cancellationToken);

        return Results.Ok(result);
    }
}
