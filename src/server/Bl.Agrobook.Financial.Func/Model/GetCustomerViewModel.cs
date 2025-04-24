using System.Text.Json.Serialization;

namespace Bl.Agrobook.Financial.Func.Model;

public class GetCustomerViewModel
{
    [JsonPropertyName("content")]
    public List<CustomerViewModel> Content { get; set; } = new();

    [JsonPropertyName("pageable")]
    public PagenableViewModel Pageable { get; set; } = new();

    [JsonPropertyName("total")]
    public int? Total { get; set; }

    [JsonPropertyName("totalElements")]
    public int? TotalElements { get; set; }

    [JsonPropertyName("totalPages")]
    public int? TotalPages { get; set; }

    [JsonPropertyName("last")]
    public bool? Last { get; set; }

    [JsonPropertyName("size")]
    public int? Size { get; set; }

    [JsonPropertyName("number")]
    public int? Number { get; set; }

    [JsonPropertyName("sort")]
    public Sort Sort { get; set; } = new();

    [JsonPropertyName("numberOfElements")]
    public int? NumberOfElements { get; set; }

    [JsonPropertyName("first")]
    public bool? First { get; set; }

    [JsonPropertyName("empty")]
    public bool? Empty { get; set; }
}
