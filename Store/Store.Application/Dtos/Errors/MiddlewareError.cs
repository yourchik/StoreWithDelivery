using System.Text.Json;

namespace Store.Application.Dtos.Errors;

public record MiddlewareError(string Message, int StatusCode)
{
    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}
