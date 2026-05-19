namespace PulseFlow.Api.Common.Models;

public class ErrorResponse
{
    public string Type { get; set; } = string.Empty;
    public  string Title { get; set; } = string.Empty;
    public int Status { get; set; }
    public string? Detail { get; set; }
    public Dictionary<string, string[]>? Errors { get; set; }
    public string TraceId { get; set; } = string.Empty;

    public static ErrorResponse Create(string type, string title, int status, string? detail = null, Dictionary<string, string[]>? errors = null, string traceId = "")
    {
        return new ErrorResponse
        {
            Type = type,
            Title = title,
            Status = status,
            Detail = detail,
            Errors = errors,
            TraceId = traceId
        };
    }

    public static ErrorResponse ValidateError(Dictionary<string, string[]> errors, string? traceId = null)
    {
        return Create(
            type: "ValidationError",
            title: "Erro de validação.",
            status: StatusCodes.Status400BadRequest,
            detail: "One or more validation errors occurred.",
            errors: errors,
            traceId: traceId ?? string.Empty
        );
    }

    public static ErrorResponse NotFound(string? detail = null, string? traceId = null)
    {
        return Create(
            type: "NotFound",
            title: "Recurso não encontrado.",
            status: StatusCodes.Status404NotFound,
            detail: detail,
            traceId: traceId ?? string.Empty
        );
    }

    public static ErrorResponse BadRequest(string? detail = null, string? traceId = null)
    {
        return Create(
            type: "BadRequest",
            title: "Requisição inválida.",
            status: StatusCodes.Status400BadRequest,
            detail: detail,
            traceId: traceId ?? string.Empty
        );
    }

    public static ErrorResponse Unauthorized(string? detail = null, string? traceId = null)
    {
        return Create(
            type: "Unauthorized",
            title: "Não autorizado.",
            status: StatusCodes.Status401Unauthorized,
            detail: detail,
            traceId: traceId ?? string.Empty
        );
    }

    public static ErrorResponse InternalServerError(string? detail = null, string? traceId = null)
    {
        return Create(
            type: "InternalServerError",
            title: "Erro interno do servidor.",
            status: StatusCodes.Status500InternalServerError,
            detail: detail,
            traceId: traceId ?? string.Empty
        );
    }
}
