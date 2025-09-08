using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Bl.Agrobook.Financial.Func.Model.Kyte;
internal class FinishSaleRequestModel
{
    [JsonPropertyName("payments")]
    public List<FinishSalePaymentRequestModel> Payments { get; set; } = [];

    [JsonPropertyName("observation")]
    public string? Observation { get; set; }

    [JsonPropertyName("status")]
    public string Status { get; set; } = "opened";

    [JsonPropertyName("showObservationInReceipt")]
    public bool ShowObservationInReceipt { get; set; } = false;

    [JsonPropertyName("date")]
    public DateTime Date { get; set; } // TODO: Check field before submitting the cart

    [JsonPropertyName("dateInt")]
    public int DateInt { get; set; } // TODO: Check field before submitting the cart

    [JsonPropertyName("dateLocal")]
    public DateTime DateLocal { get; set; } = DateTime.UtcNow;

    [JsonPropertyName("currencyCode")]
    public string CurrencyCode { get; set; } = "RS"; // TODO: Check currencty

    [JsonPropertyName("did")]
    public string Did { get; set; } = "1";

    [JsonPropertyName("totalPay")]
    public decimal TotalPay { get; set; }

    [JsonPropertyName("payBack")]
    public int PayBack { get; set; }
}
public class FinishSalePaymentRequestModel
{
    public static FinishSalePaymentRequestModel CreateOther(decimal total, decimal totalPaid)
    {
        return new FinishSalePaymentRequestModel()
        {
            Description = "Others",
            Type = 5,
            Icon = "Others",
            Placeholder = "List here the other payment methods accepted.",
            Transaction = null,
            Total = total,
            TotalPaid = totalPaid,
        };
    }

    [JsonPropertyName("type")]
    public int Type { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("icon")]
    public string? Icon { get; set; }

    [JsonPropertyName("placeholder")]
    public string? Placeholder { get; set; }

    [JsonPropertyName("testIDs")]
    public TestIDs TestIDs { get; set; }

    [JsonPropertyName("transaction")]
    public object? Transaction { get; set; }

    [JsonPropertyName("total")]
    public decimal? Total { get; set; }

    [JsonPropertyName("totalPaid")]
    public decimal? TotalPaid { get; set; }
}

public class TestIDs
{
    public static TestIDs Default = new TestIDs()
    {
        Option = "method-others"
    };

    [JsonPropertyName("option")]
    public string? Option { get; set; }
}