using Asp.Versioning.Builder;

namespace PulseFlow.Api.Endpoints.V1;

public static class WeatherForecastEndpoints
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    public static IEndpointRouteBuilder MapWeatherForecastsV1(this IEndpointRouteBuilder app, ApiVersionSet version)
    {

        var group = app.MapGroup("/api/v{version:apiVersion}/forecast")
            .WithApiVersionSet(version)
            .WithTags("WeatherForecasts")
            .WithOpenApi();

        group.MapGet("/", GetAll)
            .MapToApiVersion(1.0)
            .WithName("Consulta tempo")
            .WithSummary("Consulta a previsão do tempo ")
            .WithDescription("Endpoint para consulta a previsão do tempo")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);

        return app;
    }

    public static async Task<IResult>GetAll(CancellationToken cancellationToken)
    {
        return Results.Ok(Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        }).ToArray());
    }
}
