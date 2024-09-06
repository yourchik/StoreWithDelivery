using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Store.Application.ModelsDto.Errors;

namespace Store.Application.Middleware;

public class ExecutionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExecutionHandlingMiddleware> _logger;
    private readonly ExceptionHandlerMapping _exceptionHandlerMapping;

    public ExecutionHandlingMiddleware(
        RequestDelegate next, 
        ILogger<ExecutionHandlingMiddleware> logger,
        ExceptionHandlerMapping exceptionHandlerMapping)
    {
        _exceptionHandlerMapping = exceptionHandlerMapping;
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext, ex);
        }
    }
    
    private async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
    {
        _logger.LogError(exception, exception.Message);
        var response = httpContext.Response;
        response.ContentType = "application/json";
        var handler = _exceptionHandlerMapping.GetHandler(exception);
        response.StatusCode = (int)handler.HttpStatusCode;
        var middlewareError = new MiddlewareError(handler.Message, (int)handler.HttpStatusCode);
        var result = JsonSerializer.Serialize(middlewareError);
        await response.WriteAsync(result);
    }
    
 }