namespace Store.Domain.Entities;

public enum OrderStatus
{
    Created,
    Accepted,
    Sent,
    Delivered,
    Received,
    Cancelled
}