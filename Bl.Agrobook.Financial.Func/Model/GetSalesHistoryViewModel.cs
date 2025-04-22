using System.Text.Json.Serialization;

namespace Bl.Agrobook.Financial.Func.Model;

public class GetSalesHistoryViewModel
{
    [JsonPropertyName("totalQty")]
    public int? TotalQty { get; set; }

    [JsonPropertyName("totalValue")]
    public double? TotalValue { get; set; }

    [JsonPropertyName("sales")]
    public SalesHistoryResultViewModel Sales { get; set; } = new();
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
    public double? Commission { get; set; }

    [JsonPropertyName("status")]
    public int? Status { get; set; }

    [JsonPropertyName("devolution")]
    public bool? Devolution { get; set; }

    [JsonPropertyName("deliver")]
    public bool? Deliver { get; set; }

    [JsonPropertyName("delivery_tax")]
    public double? DeliveryTax { get; set; }

    [JsonPropertyName("seller")]
    public Seller Seller { get; set; } = new();

    [JsonPropertyName("origin")]
    public int? Origin { get; set; }

    [JsonPropertyName("tax")]
    public Tax Tax { get; set; } = new();

    [JsonPropertyName("prorated_discount")]
    public double? ProratedDiscount { get; set; }

    [JsonPropertyName("items_discount")]
    public double? ItemsDiscount { get; set; }

    [JsonPropertyName("discount")]
    public double? Discount { get; set; }

    [JsonPropertyName("percentual_sale")]
    public bool? PercentualSale { get; set; }

    [JsonPropertyName("percentage_discount")]
    public double? PercentageDiscount { get; set; }

    [JsonPropertyName("net_value")]
    public double? NetValue { get; set; }

    [JsonPropertyName("gross_value")]
    public double? GrossValue { get; set; }

    [JsonPropertyName("final_value")]
    public double? FinalValue { get; set; }

    [JsonPropertyName("obs")]
    public string? Obs { get; set; }

    [JsonPropertyName("code")]
    public string Code { get; set; } = string.Empty;

    [JsonPropertyName("payd")]
    public double? Payd { get; set; }

    [JsonPropertyName("profit")]
    public double? Profit { get; set; }

    [JsonPropertyName("debt")]
    public double? Debt { get; set; }

    [JsonPropertyName("credit")]
    public double? Credit { get; set; }

    [JsonPropertyName("credit_value")]
    public double? CreditValue { get; set; }

    [JsonPropertyName("used_credit")]
    public double? UsedCredit { get; set; }

    [JsonPropertyName("qty")]
    public double? Qty { get; set; }

    [JsonPropertyName("change")]
    public double? Change { get; set; }

    [JsonPropertyName("customer")]
    public Customer Customer { get; set; } = new();

    [JsonPropertyName("products")]
    public List<Product> Products { get; set; } = [];

    [JsonPropertyName("pending_payment")]
    public bool? PendingPayment { get; set; }
}