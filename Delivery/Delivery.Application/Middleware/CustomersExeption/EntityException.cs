namespace Delivery.Application.Middleware.CustomersExeption;

public class EntityException : Exception
{
    public EntityException() { }
    public EntityException(string message, string entityName)
        : base($"{message}") { }
    
    public EntityException(string message, Exception ex)
        : base($"{message}") { }
}


