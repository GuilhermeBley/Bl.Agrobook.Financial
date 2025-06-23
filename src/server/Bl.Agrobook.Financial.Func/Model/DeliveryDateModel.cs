using MongoDB.Bson.Serialization.Attributes;

namespace Bl.Agrobook.Financial.Func.Model;

[BsonIgnoreExtraElements]
public class DeliveryDateModel
{
    [BsonId]
    public string Id { get; set; } = string.Empty;
    public DateOnly DeliveryAt { get; set; }
    public DateTime InsertedAt { get; set; }
    public string? UserId { get; set; }

    public static string GenerateId(DateOnly deliveryAt)
    {
        return $"{deliveryAt:yyyy-MM-dd}";
    }
}
