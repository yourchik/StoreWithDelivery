﻿namespace Store.Domain.Entities;

public class Product : BaseEntity
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int Amount  { get; set; }
}