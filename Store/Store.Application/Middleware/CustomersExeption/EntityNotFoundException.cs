namespace Store.Application.Middleware.CustomersExeption;

public class EntityNotFoundException : Exception
{
    public string EntityName { get; }

    public EntityNotFoundException(string message, string entityName)
        : base($"{message} Entity: {entityName}")
    {
        EntityName = entityName;
    }
}

