using Asp.Versioning.Builder;
using MediatR;
using PulseFlow.Application.UseCase.Companies.RegisterCompany;

namespace PulseFlow.Api.Endpoints.V1;

public static class CompaniesEndpoint
{


    public static IEndpointRouteBuilder MapCompaniesV1(this IEndpointRouteBuilder app, ApiVersionSet version)
    {
        var group = app.MapGroup("/api/v{version:apiVersion}/register-company")
            .WithApiVersionSet(version)
            .WithTags("Companies")
            .WithOpenApi();

        group.MapPost("/", RegisterCompany)
            .MapToApiVersion(1.0)
            .WithName("RegisterCompany")
            .WithSummary("Register a new company")
            .WithDescription("Endpoint to register a new company")
            .Produces<RegisterCompanyOutput>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);
        return app;
    }

    public static async Task<IResult> RegisterCompany(
        RegisterCompanyInput registerCompanyInput,
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(registerCompanyInput, cancellationToken);


        return Results.Created($"/api/v1/companies/{result.Id}", result);
    }
}
