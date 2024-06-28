namespace Store.Domain.Entities;

public class Order : BaseEntity
{
    public string Address { get; set; }
    public List<Product> Products { get; set; }
    public Status Status { get; set; }
}

public enum Status
{
    Created,
    Sent,
    Delivered,
    Received,
    Cancelled   
}