using Contracts.Enum;

namespace Contracts.Messages;

public record OrderStatusMessage
    (Guid OrderId, OrderStatus Status);