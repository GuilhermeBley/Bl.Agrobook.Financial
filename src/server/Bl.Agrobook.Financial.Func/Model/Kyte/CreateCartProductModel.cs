using System.Text.Json.Serialization;

namespace Bl.Agrobook.Financial.Func.Model.Kyte;

public class CreateCartProductModel
{
    [JsonPropertyName("amount")]
    public int Amount { get; set; }

    [JsonPropertyName("productId")]
    public string ProductId { get; set; } = string.Empty;

    [JsonPropertyName("unitValue")]
    public decimal UnitValue { get; set; }
    [JsonIgnore]
    public decimal TotalPrice => UnitValue * Amount;
}
