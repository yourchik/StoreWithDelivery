using System.Net;
using Store.Application.Middleware.CustomersExeption;

namespace Store.Application.Middleware;

public class ExceptionHandlerMapping
{
    private readonly Dictionary<Type, (HttpStatusCode, string)> _exceptionHandlers = new()
    {
        { typeof(ArgumentNullException), (HttpStatusCode.BadRequest, "Invalid argument provided.") },
        { typeof(NullReferenceException), (HttpStatusCode.InternalServerError, "A null reference error occurred.") },
        { typeof(EntityNotFoundException), (HttpStatusCode.InternalServerError, "The requested entity was not found.") },
        { typeof(UnauthorizedAccessException), (HttpStatusCode.Unauthorized, "Access denied.") },
    };

    public (HttpStatusCode HttpStatusCode, string Message) GetHandler(Exception exception)
    {
        return _exceptionHandlers.TryGetValue(exception.GetType(), out var handler) 
            ? handler 
            : (HttpStatusCode.InternalServerError, "An unexpected error occurred.");
    }
}