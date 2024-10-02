using Contracts.Enum;
using Delivery.Application.Services.Interfaces.Orders;

namespace Delivery.PresentationApi.Routing;

public class RoutesOrdersApi(IOrderService orderService)
{
    public void Register(WebApplication app)
    {
        app.MapPatch("/orders/{id:guid}/status/next",
            async (Guid id, OrderStatus updateStatus) =>
            {
                await orderService.NextOrderAsync(id);
                return Results.Ok($"Order ID {id} status updated to '{updateStatus}'");
            });
        
        app.MapPatch("/orders/{id:guid}/status/close",
            async (Guid id, OrderStatus updateStatus) =>
            {
                await orderService.CloseOrderAsync(id);
                return Results.Ok($"Order ID {id} status updated to '{updateStatus}'");
            });
    }
}