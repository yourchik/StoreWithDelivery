using Delivery.Application.ModelsDto;
using Delivery.Application.Services.Interfaces.Orders;
using Delivery.Infrastructure.Services.Interfaces.Sheduler;
using Delivery.Infrastructure.Settings;
using Microsoft.Extensions.Options;

namespace Delivery.Infrastructure.Services.Implementations.Sheduler.Jobs;

public class UpdateOrderStatusJob : IJob<OrderMessage>
{
    private readonly IOrderService _orderService;
    private readonly IHangFireService _hangFireService;
    private readonly HangfireSettings _hangfireSettings;
    private static readonly OrderStatus[] Statuses = {
        OrderStatus.Created,
        OrderStatus.Accepted,
        OrderStatus.Sent,
        OrderStatus.Delivered
    };
    public UpdateOrderStatusJob(IOrderService orderService,
        IHangFireService hangFireService,
        IOptions<HangfireSettings> hangfireSettings)
    {
        _orderService = orderService;
        _hangFireService = hangFireService;
        _hangfireSettings = hangfireSettings.Value;
    }

    public async Task RunAsync(OrderMessage orderMessage, CancellationToken ct = default)
    {
        var currentIndex = Array.IndexOf(Statuses, orderMessage.Status);
        if (currentIndex == -1 || orderMessage.Status == OrderStatus.Cancelled || orderMessage.Status == OrderStatus.Delivered)
            return;
        
        var nextIndex = currentIndex + 1;
        if (nextIndex >= Statuses.Length)
            return;
        
        var nextStatus = Statuses[nextIndex];
        await _orderService.UpdateOrderStatus(orderMessage, nextStatus);
        if (orderMessage.Status != OrderStatus.Delivered && orderMessage.Status != OrderStatus.Cancelled)
            _hangFireService.Execute<UpdateOrderStatusJob>(e => e.RunAsync(orderMessage, ct), _hangfireSettings.GetStatusUpdateInterval());
    }
}
