﻿using Delivery.Application.ModelsDto;

namespace Delivery.Application.Services.Interfaces.Integration;

public interface IStoreService
{
    Task SendUpdateStatusOrderAsync(OrderStatusMessage storeDto);
}