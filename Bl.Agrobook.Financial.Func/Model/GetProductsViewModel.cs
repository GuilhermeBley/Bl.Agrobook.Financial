using System.Text.Json.Serialization;

namespace Bl.Agrobook.Financial.Func.Model;

public class GetProductsViewModel
{
    [JsonPropertyName("content")]
    public List<ContentProductViewModel> Content { get; set; } = new();

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

public class ContentProductViewModel
{
    [JsonPropertyName("uid")]
    public string Uid { get; set; } = string.Empty;

    [JsonPropertyName("shopcode")]
    public string Shopcode { get; set; } = string.Empty;

    [JsonPropertyName("code")]
    public string Code { get; set; } = string.Empty;

    [JsonPropertyName("extra_code")]
    public string? ExtraCode { get; set; }

    [JsonPropertyName("cost_price")]
    public double? CostPrice { get; set; }

    [JsonPropertyName("localization")]
    public string? Localization { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("catalog_description")]
    public string? CatalogDescription { get; set; }

    [JsonPropertyName("eangtin")]
    public string? Eangtin { get; set; }

    [JsonPropertyName("has_stock")]
    public bool? HasStock { get; set; }

    [JsonPropertyName("current_stock")]
    public double? CurrentStock { get; set; }

    [JsonPropertyName("stock")]
    public double? Stock { get; set; }

    [JsonPropertyName("stockmin")]
    public double? Stockmin { get; set; }

    [JsonPropertyName("stockmax")]
    public double? Stockmax { get; set; }

    [JsonPropertyName("stock_control")]
    public bool? StockControl { get; set; }

    [JsonPropertyName("id")]
    public int? Id { get; set; }

    [JsonPropertyName("active")]
    public bool? Active { get; set; }

    [JsonPropertyName("combo_items")]
    public List<object> ComboItems { get; set; } = new();

    [JsonPropertyName("commission_percentage")]
    public double? CommissionPercentage { get; set; }

    [JsonPropertyName("commission_profit")]
    public bool? CommissionProfit { get; set; }

    [JsonPropertyName("combo")]
    public bool? Combo { get; set; }

    [JsonPropertyName("profit_margin")]
    public double? ProfitMargin { get; set; }

    [JsonPropertyName("obs")]
    public string? Obs { get; set; }

    [JsonPropertyName("can_change_price")]
    public bool? CanChangePrice { get; set; }

    [JsonPropertyName("price")]
    public double? Price { get; set; }

    [JsonPropertyName("auto_price")]
    public bool? AutoPrice { get; set; }

    [JsonPropertyName("default_markup")]
    public bool? DefaultMarkup { get; set; }

    [JsonPropertyName("price_promo")]
    public double? PricePromo { get; set; }

    [JsonPropertyName("published")]
    public bool? Published { get; set; }

    [JsonPropertyName("promotional")]
    public bool? Promotional { get; set; }

    [JsonPropertyName("fractional_sale")]
    public bool? FractionalSale { get; set; }

    [JsonPropertyName("net_weigth")]
    public double? NetWeigth { get; set; }

    [JsonPropertyName("gross_weigth")]
    public double? GrossWeigth { get; set; }

    [JsonPropertyName("category")]
    public Category Category { get; set; } = new();

    [JsonPropertyName("subcategory")]
    public Subcategory Subcategory { get; set; } = new();

    [JsonPropertyName("unit")]
    public Unit Unit { get; set; } = new();

    [JsonPropertyName("image")]
    public Image Image { get; set; } = new();

    [JsonPropertyName("brand")]
    public Brand Brand { get; set; } = new();

    [JsonPropertyName("reserved_stock")]
    public double? ReservedStock { get; set; }

    [JsonPropertyName("photos")]
    public List<object> Photos { get; set; } = new();

    [JsonPropertyName("no_tax")]
    public bool? NoTax { get; set; }

    [JsonPropertyName("tax_use_global")]
    public bool? TaxUseGlobal { get; set; }
}