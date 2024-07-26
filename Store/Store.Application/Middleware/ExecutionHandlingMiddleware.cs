using System.Net;
using System.Reflection.Metadata;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Store.Application.Dtos.Errors;

namespace Store.Application.Middleware;

public class ExecutionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExecutionHandlingMiddleware> _logger;

    public ExecutionHandlingMiddleware(
        RequestDelegate next, 
        ILogger<ExecutionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (ArgumentNullException ex)
        {
            await HandleExceptionAsync(httpContext, ex.Message, 
                HttpStatusCode.BadRequest, "Invalid argument provided.");
        }
        catch (UnauthorizedAccessException ex)
        {
            await HandleExceptionAsync(httpContext, ex.Message,
                HttpStatusCode.Unauthorized, "Access denied.");
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext, ex.Message,
                HttpStatusCode.InternalServerError, "An unexpected error occurred.");
        }
    }
    
    private async Task HandleExceptionAsync(
        HttpContext httpContent, string exceptionMessage, 
        HttpStatusCode httpStatusCode, string message)
    {
        _logger.LogError(exceptionMessage);
        var response = httpContent.Response;
        response.ContentType = "application/json";
        response.StatusCode = (int)httpStatusCode;

        var middlewareError = new MiddlewareError(message, (int)httpStatusCode);
        var result = JsonSerializer.Serialize(middlewareError);
        await response.WriteAsync(result);
    }
    
 }