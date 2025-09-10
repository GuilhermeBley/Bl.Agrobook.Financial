using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Bl.Agrobook.Financial.Func.Model.Kyte;
public class GetSalesModel
{
    [JsonPropertyName("_sales")]
    public List<GetSaleModel> Sales { get; set; } = [];
}

public class GetSaleModel
{
    [JsonPropertyName("_id")]
    public string? Id { get; set; }

    [JsonPropertyName("active")]
    public bool? Active { get; set; }

    [JsonPropertyName("aid")]
    public string? Aid { get; set; }

    [JsonPropertyName("currencyCode")]
    public string? CurrencyCode { get; set; }

    [JsonPropertyName("customer")]
    public GetSaleCustomerModel? Customer { get; set; }

    [JsonPropertyName("dateClosed")]
    public object? DateClosed { get; set; }

    [JsonPropertyName("dateClosedInt")]
    public object? DateClosedInt { get; set; }

    [JsonPropertyName("dateClosedLocal")]
    public object? DateClosedLocal { get; set; }

    [JsonPropertyName("dateCreation")]
    public DateTime? DateCreation { get; set; }

    [JsonPropertyName("dateCreationInt")]
    public int? DateCreationInt { get; set; }

    [JsonPropertyName("deviceCountry")]
    public string? DeviceCountry { get; set; }

    [JsonPropertyName("did")]
    public string? Did { get; set; }

    [JsonPropertyName("discountPercent")]
    public decimal? DiscountPercent { get; set; }

    [JsonPropertyName("discountValue")]
    public decimal? DiscountValue { get; set; }

    [JsonPropertyName("gatewayKey")]
    public object? GatewayKey { get; set; }

    [JsonPropertyName("isCancelled")]
    public bool? IsCancelled { get; set; }

    [JsonPropertyName("isSenderReceipt")]
    public bool? IsSenderReceipt { get; set; }

    [JsonPropertyName("items")]
    public List<GetSaleItemModel> Items { get; set; } = [];

    [JsonPropertyName("lang")]
    public object? Lang { get; set; }

    [JsonPropertyName("number")]
    public int Number { get; set; }

    [JsonPropertyName("observation")]
    public string? Observation { get; set; }

    [JsonPropertyName("origin")]
    public object? Origin { get; set; }

    [JsonPropertyName("payBack")]
    public object? PayBack { get; set; }

    [JsonPropertyName("paymentLink")]
    public object? PaymentLink { get; set; }

    [JsonPropertyName("payments")]
    public List<object>? Payments { get; set; }

    [JsonPropertyName("prevStatus")]
    public object? PrevStatus { get; set; }

    [JsonPropertyName("showObservationInReceipt")]
    public bool? ShowObservationInReceipt { get; set; }

    [JsonPropertyName("sid")]
    public string? Sid { get; set; }

    [JsonPropertyName("status")]
    public string? Status { get; set; }

    [JsonPropertyName("statusInfo")]
    public object? StatusInfo { get; set; }

    [JsonPropertyName("taxes")]
    public List<object>? Taxes { get; set; }

    [JsonPropertyName("timeline")]
    public List<object>? Timeline { get; set; }

    [JsonPropertyName("toDeliver")]
    public bool? ToDeliver { get; set; }

    [JsonPropertyName("totalGross")]
    public decimal? TotalGross { get; set; }

    [JsonPropertyName("totalNet")]
    public decimal? TotalNet { get; set; }

    [JsonPropertyName("totalPay")]
    public decimal? TotalPay { get; set; }

    [JsonPropertyName("totalProfit")]
    public decimal? TotalProfit { get; set; }

    [JsonPropertyName("totalTaxes")]
    public decimal? TotalTaxes { get; set; }

    [JsonPropertyName("uid")]
    public string? Uid { get; set; }

    [JsonPropertyName("userEmail")]
    public string? UserEmail { get; set; }

    [JsonPropertyName("userName")]
    public string? UserName { get; set; }

    [JsonPropertyName("shippingFee")]
    public object? ShippingFee { get; set; }
}

public class GetSaleCustomerModel
{
    [JsonPropertyName("_id")]
    public string? Id { get; set; }

    [JsonPropertyName("accountBalance")]
    public object? AccountBalance { get; set; }

    [JsonPropertyName("active")]
    public bool? Active { get; set; }

    [JsonPropertyName("address")]
    public object? Address { get; set; }

    [JsonPropertyName("addressComplement")]
    public object? AddressComplement { get; set; }

    [JsonPropertyName("aid")]
    public string? Aid { get; set; }

    [JsonPropertyName("allowPayLater")]
    public bool? AllowPayLater { get; set; }

    [JsonPropertyName("celPhone")]
    public object? CelPhone { get; set; }

    [JsonPropertyName("dateClosed")]
    public object? DateClosed { get; set; }

    [JsonPropertyName("dateCreation")]
    public object? DateCreation { get; set; }

    [JsonPropertyName("documentNumber")]
    public string? DocumentNumber { get; set; }

    [JsonPropertyName("email")]
    public object? Email { get; set; }

    [JsonPropertyName("env")]
    public object? Env { get; set; }

    [JsonPropertyName("image")]
    public object? Image { get; set; }

    [JsonPropertyName("isGuest")]
    public bool? IsGuest { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("observation")]
    public object? Observation { get; set; }

    [JsonPropertyName("phone")]
    public object? Phone { get; set; }

    [JsonPropertyName("salesQuantity")]
    public int? SalesQuantity { get; set; }

    [JsonPropertyName("search")]
    public string? Search { get; set; }

    [JsonPropertyName("totalSalesClosed")]
    public int? TotalSalesClosed { get; set; }

    [JsonPropertyName("totalSalesOpened")]
    public int? TotalSalesOpened { get; set; }

    [JsonPropertyName("uid")]
    public string? Uid { get; set; }

    [JsonPropertyName("userName")]
    public string? UserName { get; set; }
}

public class GetSaleItemModel
{
    [JsonPropertyName("amount")]
    public decimal? Amount { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("discount")]
    public object? Discount { get; set; }

    [JsonPropertyName("fraction")]
    public decimal? Fraction { get; set; }

    [JsonPropertyName("grossValue")]
    public decimal? GrossValue { get; set; }

    [JsonPropertyName("product")]
    public GetSaleProductModel Product { get; set; } = new();

    [JsonPropertyName("profitValue")]
    public decimal? ProfitValue { get; set; }

    [JsonPropertyName("unitValue")]
    public decimal? UnitValue { get; set; }

    [JsonPropertyName("value")]
    public decimal? Value { get; set; }

}

public class GetSaleProductModel
{
    [JsonPropertyName("code")]
    public string? Code { get; set; }

    [JsonPropertyName("costValue")]
    public decimal? CostValue { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("image")]
    public object? Image { get; set; }

    [JsonPropertyName("imagePath")]
    public object? ImagePath { get; set; }

    [JsonPropertyName("isFractioned")]
    public bool? IsFractioned { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("originalUnitValue")]
    public decimal? OriginalUnitValue { get; set; }

    [JsonPropertyName("prodId")]
    public string? ProdId { get; set; }

    [JsonPropertyName("stockActive")]
    public bool? StockActive { get; set; }

    [JsonPropertyName("unitValue")]
    public decimal? UnitValue { get; set; }
}