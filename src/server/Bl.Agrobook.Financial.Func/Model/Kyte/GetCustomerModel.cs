using System.Text.Json.Serialization;

namespace Bl.Agrobook.Financial.Func.Model.Kyte;

public class GetCustomerCustomerModel
{
    [JsonPropertyName("_id")]
    public string? Id { get; set; }

    [JsonPropertyName("active")]
    public bool? Active { get; set; }

    [JsonPropertyName("aid")]
    public string? Aid { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("celPhone")]
    public string? CelPhone { get; set; }

    [JsonPropertyName("phone")]
    public string? Phone { get; set; }

    [JsonPropertyName("dateCreation")]
    public DateTime? DateCreation { get; set; }

    [JsonPropertyName("email")]
    public string? Email { get; set; }

    [JsonPropertyName("address")]
    public string? Address { get; set; }

    [JsonPropertyName("addressComplement")]
    public string? AddressComplement { get; set; }

    [JsonPropertyName("accountBalance")]
    public int? AccountBalance { get; set; }

    [JsonPropertyName("image")]
    public object? Image { get; set; }

    [JsonPropertyName("uid")]
    public string Uid { get; set; } = string.Empty;

    [JsonPropertyName("observation")]
    public string? Observation { get; set; }

    [JsonPropertyName("allowPayLater")]
    public bool? AllowPayLater { get; set; }

    [JsonPropertyName("documentNumber")]
    public string? DocumentNumber { get; set; }
}

public class GetCustomerModel
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("_customers")]
    public List<GetCustomerCustomerModel> Customers { get; set; } = [];
}