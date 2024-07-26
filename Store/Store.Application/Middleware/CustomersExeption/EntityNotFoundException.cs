namespace Store.Application.Middleware.CustomersExeption;

public class EntityNotFoundException : Exception
{
    public EntityNotFoundException(string message) : base(message) { }
}