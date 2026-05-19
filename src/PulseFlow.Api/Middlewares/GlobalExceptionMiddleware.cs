using PulseFlow.Api.Common.Models;
using System.Net;

namespace PulseFlow.Api.Middlewares;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger, IWebHostEnvironment webHostEnvironment)
    {
        _next = next;
        _logger = logger;
        _webHostEnvironment = webHostEnvironment;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var traceId = context.TraceIdentifier;
        var path = context.Request.Path;
        var method = context.Request.Method;
        ErrorResponse errorResponse;

        switch (exception)
        {
            case ArgumentException argEx:
                _logger.LogWarning(argEx,
                    "Bad Request: {Message} | TraceId: {TraceId} | Path: {Path} | Method: {Method}",
                    argEx.Message, traceId, path, method);

                errorResponse = ErrorResponse.BadRequest(argEx.Message, traceId);
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                break;

            case KeyNotFoundException keyNotFoundEx:
                _logger.LogWarning(keyNotFoundEx,
                    "Resource Not Found: {Message} | TraceId: {TraceId} | Path: {Path} | Method: {Method}",
                    keyNotFoundEx.Message, traceId, path, method);

                errorResponse = ErrorResponse.NotFound(keyNotFoundEx.Message, traceId);
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                break;

            case InvalidOperationException invalidOperationException:
                _logger.LogWarning(invalidOperationException,
                    "Invalid Operation: {Message} | TraceId: {TraceId} | Path: {Path} | Method: {Method}",
                    invalidOperationException.Message, traceId, path, method);

                errorResponse = ErrorResponse.BadRequest(invalidOperationException.Message, traceId);
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                break;

            case NotImplementedException notImplemented:
                _logger.LogWarning(notImplemented,
                    "Not Implemented: {Message} | TraceId: {TraceId} | Path: {Path} | Method: {Method}",
                    notImplemented.Message, traceId, path, method);

                errorResponse = ErrorResponse.NotFound(notImplemented.Message, traceId);
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                break;

            default:
                _logger.LogError(exception,
                    "Unhandled Exception: {ExceptionType} | TraceId: {TraceId} | Path: {Path} | Method: {Method} | StatusCode: {StatusCode}",
                    exception.GetType().Name, traceId, path, method, (int)HttpStatusCode.InternalServerError);

                errorResponse = ErrorResponse.Create(
                    type: "InternalServerError",
                    title: "An unexpected error occurred.",
                    status: (int)HttpStatusCode.InternalServerError,
                    detail: _webHostEnvironment.IsDevelopment() ? exception.ToString() : null,
                    traceId: traceId
                );
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                break;
        }

        context.Response.ContentType = "application/json";
        await context.Response.WriteAsJsonAsync(errorResponse);
    }
}
