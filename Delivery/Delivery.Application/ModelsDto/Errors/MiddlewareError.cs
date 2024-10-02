using System.Text.Json;

namespace Delivery.Application.ModelsDto.Errors;

public record MiddlewareError(string Message, int StatusCode)
{
    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}
