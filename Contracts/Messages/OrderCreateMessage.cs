using Contracts.Enum;

namespace Contracts.Messages;

public record OrderCreateMessage(
    Guid Id, string Address, 
    OrderStatus Status);
