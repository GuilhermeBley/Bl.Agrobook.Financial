using System.Text.Json.Serialization;

namespace Bl.Agrobook.Financial.Func.Model;

public class CustomerViewModel
{
    [JsonPropertyName("uid")]
    public string Uid { get; set; } = string.Empty;

    [JsonPropertyName("shopcode")]
    public string Shopcode { get; set; } = string.Empty;

    [JsonPropertyName("code")]
    public string Code { get; set; } = string.Empty;

    [JsonPropertyName("ddi")]
    public string? Ddi { get; set; }

    [JsonPropertyName("debit_value")]
    public double? DebitValue { get; set; }

    [JsonPropertyName("credit_value")]
    public double? CreditValue { get; set; }

    [JsonPropertyName("flag")]
    public object? Flag { get; set; }

    [JsonPropertyName("id")]
    public int? Id { get; set; }

    [JsonPropertyName("active")]
    public bool? Active { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("cpf_cnpj")]
    public string? CpfCnpj { get; set; }

    [JsonPropertyName("rg")]
    public string? Rg { get; set; }

    [JsonPropertyName("physical_person")]
    public bool? PhysicalPerson { get; set; }

    [JsonPropertyName("juridical_person")]
    public bool? JuridicalPerson { get; set; }

    [JsonPropertyName("gender")]
    public string? Gender { get; set; }

    [JsonPropertyName("mother")]
    public string? Mother { get; set; }

    [JsonPropertyName("father")]
    public string? Father { get; set; }

    [JsonPropertyName("extra_info")]
    public string? ExtraInfo { get; set; }

    [JsonPropertyName("email")]
    public string? Email { get; set; }

    [JsonPropertyName("obs")]
    public string? Obs { get; set; }

    [JsonPropertyName("phone")]
    public string? Phone { get; set; }

    [JsonPropertyName("address_id")]
    public string? AddressId { get; set; }

    [JsonPropertyName("addresses")]
    public List<AddressViewModel> Addresses { get; set; } = new();

    [JsonPropertyName("created_at")]
    public DateTime? CreatedAt { get; set; }
}
