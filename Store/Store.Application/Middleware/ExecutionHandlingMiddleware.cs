using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Store.Application.Middleware;

public class ExecutionHandlingMiddleware(
    RequestDelegate next,
    ILogger<ExecutionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await next(httpContext);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext, ex.Message);
        }
    }
    
    private async Task HandleExceptionAsync(HttpContext httpContext, 
        string exMessage)
    {
        logger.LogError(exMessage);
        var response = httpContext.Response;
        response.ContentType = "application/json";
        response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var errorResponse = new
        {
            Message = exMessage,
            StatusCode = (int)HttpStatusCode.InternalServerError
        };
        
        var result = JsonSerializer.Serialize(errorResponse);
        await response.WriteAsync(result);
    }
 }