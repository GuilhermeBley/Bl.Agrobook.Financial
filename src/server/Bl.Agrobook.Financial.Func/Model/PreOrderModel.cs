
using MongoDB.Bson.Serialization.Attributes;

namespace Bl.Agrobook.Financial.Func.Model;

[BsonIgnoreExtraElements]
public class PreOrderModel
{
    [BsonId]
    public string Code { get; set; } = string.Empty;
    public string UserPhone { get; set; } = string.Empty;
    public string UserEmail { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    // TODO: Product list
    public DateOnly DeliveryAt { get; set; }
    public DateTime UpdateAt { get; set; }
    public DateTime InsertedAt { get; set; }
    public string? Obs { get; set; }
}
