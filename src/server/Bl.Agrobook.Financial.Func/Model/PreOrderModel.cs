
using MongoDB.Bson.Serialization.Attributes;

namespace Bl.Agrobook.Financial.Func.Model;

[BsonIgnoreExtraElements]
public class PreOrderProductModel
{
    [BsonId]
    public Guid Id { get; set; }
    public string ProductCode { get; set; } = string.Empty;
    public string ProductName { get; set; } = string.Empty;
    public decimal? Price { get; set; }
    public decimal Quantity { get; set; }
}

[BsonIgnoreExtraElements]
public class PreOrderModel
{
    [BsonId]
    public Guid Id { get; set; }
    public string? OrderCode { get; set; }
    public string CustomerPhone { get; set; } = string.Empty;
    public string CustomerEmail { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public string? CustomerCode { get; set; }
    public List<PreOrderProductModel> Product { get; set; } = [];
    public DateOnly DeliveryAt { get; set; }
    public DateTime UpdateAt { get; set; }
    public DateTime InsertedAt { get; set; }
    public string? Obs { get; set; }
}
