using System.Text.Json.Serialization;

namespace Bl.Agrobook.Financial.Func.Model;

public class GetSalesHistoryViewModel
{
    [JsonPropertyName("totalQty")]
    public int? TotalQty { get; set; }

    [JsonPropertyName("totalValue")]
    public decimal? TotalValue { get; set; }

    [JsonPropertyName("orders")]
    public SalesHistoryResultViewModel Orders { get; set; } = new();
}

public class SalesHistoryResultViewModel
{
    [JsonPropertyName("content")]
    public List<SaleHistoryViewModel> Content { get; set; } = [];

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

public class SaleHistoryViewModel
{
    [JsonPropertyName("uid")]
    public string Uid { get; set; } = string.Empty;

    [JsonPropertyName("id")]
    public int? Id { get; set; }

    [JsonPropertyName("shopcode")]
    public string Shopcode { get; set; } = string.Empty;

    [JsonPropertyName("date")]
    public DateTime? Date { get; set; }

    [JsonPropertyName("included_on")]
    public DateTime? IncludedOn { get; set; }

    [JsonPropertyName("canceled")]
    public bool? Canceled { get; set; }

    [JsonPropertyName("canceled_at")]
    public object? CanceledAt { get; set; }

    [JsonPropertyName("canceled_by")]
    public string? CanceledBy { get; set; }

    [JsonPropertyName("commission")]
    public decimal? Commission { get; set; }

    [JsonPropertyName("status")]
    public int? Status { get; set; }

    [JsonPropertyName("devolution")]
    public bool? Devolution { get; set; }

    [JsonPropertyName("deliver")]
    public bool? Deliver { get; set; }

    [JsonPropertyName("delivery_tax")]
    public decimal? DeliveryTax { get; set; }

    [JsonPropertyName("seller")]
    public Seller Seller { get; set; } = new();

    [JsonPropertyName("origin")]
    public int? Origin { get; set; }

    [JsonPropertyName("prorated_discount")]
    public decimal? ProratedDiscount { get; set; }

    [JsonPropertyName("items_discount")]
    public decimal? ItemsDiscount { get; set; }

    [JsonPropertyName("discount")]
    public decimal? Discount { get; set; }

    [JsonPropertyName("percentual_sale")]
    public bool? PercentualSale { get; set; }

    [JsonPropertyName("percentage_discount")]
    public decimal? PercentageDiscount { get; set; }

    [JsonPropertyName("net_value")]
    public decimal? NetValue { get; set; }

    [JsonPropertyName("gross_value")]
    public decimal? GrossValue { get; set; }

    [JsonPropertyName("final_value")]
    public decimal? FinalValue { get; set; }

    [JsonPropertyName("obs")]
    public string? Obs { get; set; }

    [JsonPropertyName("code")]
    public string Code { get; set; } = string.Empty;

    [JsonPropertyName("payd")]
    public decimal? Payd { get; set; }

    [JsonPropertyName("profit")]
    public decimal? Profit { get; set; }

    [JsonPropertyName("debt")]
    public decimal? Debt { get; set; }

    [JsonPropertyName("credit")]
    public decimal? Credit { get; set; }

    [JsonPropertyName("credit_value")]
    public decimal? CreditValue { get; set; }

    [JsonPropertyName("used_credit")]
    public decimal? UsedCredit { get; set; }

    [JsonPropertyName("qty")]
    public decimal? Qty { get; set; }

    [JsonPropertyName("change")]
    public decimal? Change { get; set; }

    [JsonPropertyName("customer")]
    public Customer Customer { get; set; } = new();

    [JsonPropertyName("products")]
    public List<SaleProduct> Products { get; set; } = new();

    [JsonPropertyName("pending_payment")]
    public bool? PendingPayment { get; set; }
}

public class SaleProduct
{
    [JsonPropertyName("uid")]
    public string Uid { get; set; } = string.Empty;

    [JsonPropertyName("uid_itran")]
    public string? UidItran { get; set; }

    [JsonPropertyName("uid_movest")]
    public string? UidMovest { get; set; }

    [JsonPropertyName("code")]
    public string Code { get; set; } = string.Empty;

    [JsonPropertyName("commission")]
    public decimal? Commission { get; set; }

    [JsonPropertyName("commission_percentage")]
    public decimal? CommissionPercentage { get; set; }

    [JsonPropertyName("commission_profit")]
    public bool? CommissionProfit { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("qty")]
    public decimal? Qty { get; set; }

    [JsonPropertyName("price")]
    public decimal? Price { get; set; }

    [JsonPropertyName("discount")]
    public decimal? Discount { get; set; }

    [JsonPropertyName("prorated_discount")]
    public decimal? ProratedDiscount { get; set; }

    [JsonPropertyName("item_discount")]
    public decimal? ItemDiscount { get; set; }

    [JsonPropertyName("percentual_sale")]
    public bool? PercentualSale { get; set; }

    [JsonPropertyName("percentage_discount")]
    public decimal? PercentageDiscount { get; set; }

    [JsonPropertyName("gross_value")]
    public decimal? GrossValue { get; set; }

    [JsonPropertyName("net_value")]
    public decimal? NetValue { get; set; }

    [JsonPropertyName("final_value")]
    public decimal? FinalValue { get; set; }

    [JsonPropertyName("debt")]
    public decimal? Debt { get; set; }

    [JsonPropertyName("payd")]
    public decimal? Payd { get; set; }

    [JsonPropertyName("fractional_sale")]
    public bool? FractionalSale { get; set; }

    [JsonPropertyName("profit")]
    public decimal? Profit { get; set; }

    [JsonPropertyName("cost_price")]
    public decimal? CostPrice { get; set; }

    [JsonPropertyName("cost_total")]
    public decimal? CostTotal { get; set; }

    [JsonPropertyName("combo_parent")]
    public bool? ComboParent { get; set; }

    [JsonPropertyName("combo_child")]
    public bool? ComboChild { get; set; }

    [JsonPropertyName("uid_combo_parent")]
    public string? UidComboParent { get; set; }

    [JsonPropertyName("image_url")]
    public string? ImageUrl { get; set; }

    [JsonPropertyName("category")]
    public object? Category { get; set; }

    [JsonPropertyName("unit")]
    public object? Unit { get; set; }

    [JsonPropertyName("obs")]
    public string? Obs { get; set; }
}