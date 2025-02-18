﻿using Contracts.Enum;

namespace Store.Application.ModelsDto.Orders;

public class OutputOrderDto
{
    public required Guid Id { get; init; }
    public required string Address { get; init; }
    public required Guid UserId { get; init; }
    public required IEnumerable<Guid> Products { get; init; }
    public OrderStatus Status { get; init; }
}