using Contracts.Messages;
using Delivery.Application.ModelsDto.Orders;
using Delivery.Application.Services.Interfaces.Orders;
using Delivery.Infrastructure.Services.Implementations.RabbitMQ;
using Delivery.Infrastructure.Services.Interfaces.RabbitMQ;
using Delivery.Infrastructure.Services.Interfaces.Scheduler;
using Microsoft.Extensions.Logging;

namespace Delivery.Infrastructure.Services.Implementations.Scheduler.Jobs;

public class GetNewOrdersJob
(IOrderService orderService, 
    IRabbitMqMessageService rabbitMqMessageService,
    ILogger<GetNewOrdersJob> logger,
    RabbitMqProducerService rabbitMqProducerService) 
    : IJob
{
    public async Task RunAsync(CancellationToken ct = default)
    {
        logger.LogInformation("FetchNewOrdersJob started at {Time}", DateTime.UtcNow);
        try
        {
            logger.LogInformation("Fetching messages from RabbitMQ...");
            var orderMessages = await rabbitMqMessageService.GetMessagesAsync(ct);
            var orderDtos = orderMessages.ToList();
            if (orderDtos.Count == 0)
            {
                logger.LogInformation("No new messages were fetched from RabbitMQ.");
                return;
            }
            
            logger.LogInformation("Fetched {Count} messages from RabbitMQ.", orderDtos.Count);
            await orderService.CreateRangeOrdersAsync(orderDtos);
            var orderStatusMessage = orderDtos
                .Select(x => new OrderStatusMessage(x.Id, x.Status))
                .ToList();
            await rabbitMqProducerService.OrderStatusRangeUpdateAsync(orderStatusMessage);
            logger.LogInformation("Successfully created {Count} orders in the database.", orderDtos.Count);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while fetching and processing orders.");
        }
        
        logger.LogInformation("FetchNewOrdersJob finished at {Time}", DateTime.UtcNow);
    }
}
