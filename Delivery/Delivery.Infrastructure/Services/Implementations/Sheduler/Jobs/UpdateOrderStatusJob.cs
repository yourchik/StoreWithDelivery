using Delivery.Application.ModelsDto;
using Delivery.Application.Services.Interfaces.Orders;
using Delivery.Infrastructure.Services.Interfaces.Sheduler;
using Delivery.Infrastructure.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Delivery.Infrastructure.Services.Implementations.Sheduler.Jobs;

public class UpdateOrderStatusJob : IJob<Order>
{
    private readonly IOrderService _orderService;
    private readonly IHangFireService _hangFireService;
    private readonly HangfireSettings _hangfireSettings;
    private static readonly OrderStatus[] Statuses = {
        OrderStatus.Accepted,
        OrderStatus.Sent,
        OrderStatus.Delivered,
        OrderStatus.Received,
    };
    public UpdateOrderStatusJob(IOrderService orderService,
        IHangFireService hangFireService,
        IOptions<HangfireSettings> hangfireSettings)
    {
        _orderService = orderService;
        _hangFireService = hangFireService;
        _hangfireSettings = hangfireSettings.Value;
    }

    public async Task RunAsync(Order order, CancellationToken ct = default)
    {
        var currentIndex = Array.IndexOf(Statuses, order.Status);
        if (currentIndex == -1 || order.Status == OrderStatus.Cancelled || order.Status == OrderStatus.Delivered)
            return;
        
        var nextIndex = currentIndex + 1;
        if (nextIndex >= Statuses.Length)
            return;
        
        var nextStatus = Statuses[nextIndex];
        await _orderService.UpdateOrderStatus(order, nextStatus);
        if (order.Status != OrderStatus.Delivered && order.Status != OrderStatus.Cancelled)
            _hangFireService.Execute<UpdateOrderStatusJob>(e => e.RunAsync(order, ct), _hangfireSettings.GetStatusUpdateInterval());
    }
}
