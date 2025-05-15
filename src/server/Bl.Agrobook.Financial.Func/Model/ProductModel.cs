using MongoDB.Bson.Serialization.Attributes;

namespace Bl.Agrobook.Financial.Func.Model;

[BsonIgnoreExtraElements]
public class ProductModel
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal? AvailableQuantity { get; set; }
    public decimal? Price { get; set; }
    public bool Active { get; set; }
    public string? ImgUrl { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public DateTimeOffset InsertedAt { get; set; }
}
