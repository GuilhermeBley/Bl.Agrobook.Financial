using System.Text.Json.Serialization;

namespace Bl.Agrobook.Financial.Func.Model.Kyte;
public class GetProductRootModel
{
    [JsonPropertyName("count")]
    public int Count { get; set; }

    [JsonPropertyName("isWeb")]
    public string? IsWeb { get; set; }

    [JsonPropertyName("_products")]
    public List<GetProductViewModel> Products { get; set; } = [];
}

public class GetProductViewModel
{

    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("active")]
    public bool Active { get; set; }

    [JsonPropertyName("aid")]
    public string Aid { get; set; } = string.Empty;

    [JsonPropertyName("uid")]
    public string Uid { get; set; } = string.Empty;

    [JsonPropertyName("dateCreation")]
    public DateTime DateCreation { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("description")]
    public object? Description { get; set; }

    [JsonPropertyName("code")]
    public string? Code { get; set; }

    [JsonPropertyName("search")]
    public string? Search { get; set; }

    [JsonPropertyName("background")]
    public object? Background { get; set; }

    [JsonPropertyName("foreground")]
    public object? Foreground { get; set; }

    [JsonPropertyName("label")]
    public string? Label { get; set; }

    [JsonPropertyName("image")]
    public object? Image { get; set; }

    [JsonPropertyName("imageThumb")]
    public object? ImageThumb { get; set; }

    [JsonPropertyName("imageLarge")]
    public object? ImageLarge { get; set; }

    [JsonPropertyName("imageMedium")]
    public object? ImageMedium { get; set; }

    [JsonPropertyName("category")]
    public object? Category { get; set; }

    [JsonPropertyName("isFractioned")]
    public bool? IsFractioned { get; set; }

    [JsonPropertyName("saleCostPrice")]
    public int? SaleCostPrice { get; set; }

    [JsonPropertyName("salePrice")]
    public int? SalePrice { get; set; }

    [JsonPropertyName("userName")]
    public string? UserName { get; set; }

    [JsonPropertyName("showOnCatalog")]
    public bool? ShowOnCatalog { get; set; }

    [JsonPropertyName("stock")]
    public object? Stock { get; set; }

    [JsonPropertyName("pin")]
    public bool? Pin { get; set; }

    [JsonPropertyName("salePromotionalPrice")]
    public object? SalePromotionalPrice { get; set; }

    [JsonPropertyName("stockActive")]
    public bool? StockActive { get; set; }

    [JsonPropertyName("stockStatus")]
    public string? StockStatus { get; set; }

    [JsonPropertyName("gallery")]
    public List<object>? Gallery { get; set; }

    [JsonPropertyName("variations")]
    public List<object>? Variations { get; set; }

    [JsonPropertyName("variants")]
    public List<object>? Variants { get; set; }

    [JsonPropertyName("isChildren")]
    public object? IsChildren { get; set; }

    [JsonPropertyName("isParent")]
    public bool? IsParent { get; set; }

    [JsonPropertyName("parentId")]
    public object? ParentId { get; set; }

    [JsonPropertyName("lowestPrice")]
    public object? LowestPrice { get; set; }
}
