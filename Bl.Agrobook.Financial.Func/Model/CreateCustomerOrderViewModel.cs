using System.Text.Json.Serialization;

namespace Bl.Agrobook.Financial.Func.Model;

public class CreateCustomerOrderViewModel
{
    [JsonPropertyName("canceled")]
    public bool? Canceled { get; set; }

    [JsonPropertyName("id")]
    public int? Id { get; set; }

    [JsonPropertyName("change")]
    public int? Change { get; set; }

    [JsonPropertyName("included_on")]
    public DateTime? IncludedOn { get; set; }

    [JsonPropertyName("credit")]
    public int? Credit { get; set; }

    [JsonPropertyName("credit_value")]
    public int? CreditValue { get; set; }

    [JsonPropertyName("debt")]
    public int? Debt { get; set; }

    [JsonPropertyName("customer")]
    public Customer Customer { get; set; } = new();

    [JsonPropertyName("deliver")]
    public bool? Deliver { get; set; }

    [JsonPropertyName("delivery_tax")]
    public int? DeliveryTax { get; set; }

    /// <summary>
    /// Default empty transporter
    /// </summary>
    [JsonPropertyName("transporter")]
    public Transporter Transporter { get; set; } = Transporter.Empty;

    [JsonPropertyName("devolution")]
    public bool? Devolution { get; set; }

    [JsonPropertyName("discount")]
    public int? Discount { get; set; }

    [JsonPropertyName("final_value")]
    public int? FinalValue { get; set; }

    [JsonPropertyName("gross_value")]
    public int? GrossValue { get; set; }

    [JsonPropertyName("net_value")]
    public int? NetValue { get; set; }

    [JsonPropertyName("items_discount")]
    public int? ItemsDiscount { get; set; }

    [JsonPropertyName("obs")]
    public string? Obs { get; set; }

    [JsonPropertyName("origin")]
    public int? Origin { get; set; }

    [JsonPropertyName("payd")]
    public int? Payd { get; set; }

    [JsonPropertyName("payments")]
    public List<object> Payments { get; set; } = new();

    [JsonPropertyName("pending_payment")]
    public bool? PendingPayment { get; set; }

    [JsonPropertyName("percentage_discount")]
    public int? PercentageDiscount { get; set; }

    [JsonPropertyName("percentual_sale")]
    public bool? PercentualSale { get; set; }

    [JsonPropertyName("products")]
    public List<Product> Products { get; set; } = new();

    [JsonPropertyName("prorated_discount")]
    public int? ProratedDiscount { get; set; }

    [JsonPropertyName("qty")]
    public int? Qty { get; set; }

    [JsonPropertyName("seller")]
    public Seller Seller { get; set; } = Seller.Default;

    [JsonPropertyName("shopcode")]
    public string? Shopcode { get; set; }

    [JsonPropertyName("status")]
    public int? Status { get; set; }

    /// <summary>
    /// Default empty address
    /// </summary>
    [JsonPropertyName("tax")]
    public Tax Tax { get; set; } = Tax.Default;

    [JsonPropertyName("uid")]
    public string? Uid { get; set; }

    [JsonPropertyName("used_credit")]
    public int? UsedCredit { get; set; }
}

public class Address
{
    [JsonPropertyName("district")]
    public string? District { get; set; }

    [JsonPropertyName("zipcode")]
    public string? Zipcode { get; set; }

    [JsonPropertyName("city")]
    public string? City { get; set; }

    [JsonPropertyName("city_code")]
    public string? CityCode { get; set; }

    [JsonPropertyName("number")]
    public string? Number { get; set; }

    [JsonPropertyName("street")]
    public string? Street { get; set; }

    [JsonPropertyName("complement")]
    public string? Complement { get; set; }

    [JsonPropertyName("address_id")]
    public string? AddressId { get; set; }

    [JsonPropertyName("state")]
    public string? State { get; set; }

    [JsonPropertyName("country")]
    public string? Country { get; set; }
}

public class Brand
{
}

public class Category
{
}

public class Customer
{
    [JsonPropertyName("address")]
    public Address Address { get; set; } = new();

    [JsonPropertyName("document")]
    public string? Document { get; set; }

    [JsonPropertyName("email")]
    public string? Email { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("phone")]
    public string? Phone { get; set; }

    [JsonPropertyName("ddi")]
    public string? Ddi { get; set; }

    [JsonPropertyName("uid")]
    public string? Uid { get; set; }
}

public class Image
{
    public static Image Default => new Image()
    {
        Url = string.Empty,
        Status = 3,
        Md5 = string.Empty
    };

    [JsonPropertyName("url")]
    public string? Url { get; set; }

    [JsonPropertyName("status")]
    public int? Status { get; set; }

    [JsonPropertyName("md5")]
    public string? Md5 { get; set; }
}

public class Product
{
    [JsonPropertyName("code")]
    public string? Code { get; set; }

    [JsonPropertyName("combo_child")]
    public bool? ComboChild { get; set; }

    [JsonPropertyName("combo_parent")]
    public bool? ComboParent { get; set; }

    [JsonPropertyName("debt")]
    public int? Debt { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("discount")]
    public int? Discount { get; set; }

    [JsonPropertyName("ean_gtin")]
    public string? EanGtin { get; set; }

    [JsonPropertyName("final_value")]
    public int? FinalValue { get; set; }

    [JsonPropertyName("fractional_sale")]
    public bool? FractionalSale { get; set; }

    [JsonPropertyName("gross_value")]
    public int? GrossValue { get; set; }

    [JsonPropertyName("image_url")]
    public string? ImageUrl { get; set; }

    [JsonPropertyName("item_discount")]
    public int? ItemDiscount { get; set; }

    [JsonPropertyName("net_value")]
    public int? NetValue { get; set; }

    [JsonPropertyName("obs")]
    public string? Obs { get; set; }

    [JsonPropertyName("payd")]
    public int? Payd { get; set; }

    [JsonPropertyName("percentage_discount")]
    public int? PercentageDiscount { get; set; }

    [JsonPropertyName("percentual_sale")]
    public bool? PercentualSale { get; set; }

    [JsonPropertyName("price")]
    public int? Price { get; set; }

    [JsonPropertyName("prorated_discount")]
    public int? ProratedDiscount { get; set; }

    [JsonPropertyName("qty")]
    public int? Qty { get; set; }

    [JsonPropertyName("uid")]
    public string? Uid { get; set; }

    /// <summary>
    /// Default empty tax
    /// </summary>
    [JsonPropertyName("tax")]
    public Tax Tax { get; set; } = Tax.Default;

    [JsonPropertyName("tax_details")]
    public TaxDetails TaxDetails { get; set; } = new();

    [JsonPropertyName("category")]
    public Category Category { get; set; } = new();

    [JsonPropertyName("unit")]
    public Unit Unit { get; set; } = new();

    [JsonPropertyName("code_extra")]
    public string? CodeExtra { get; set; }

    [JsonPropertyName("brand")]
    public Brand Brand { get; set; } = new();

    [JsonPropertyName("extra_code")]
    public string? ExtraCode { get; set; }

    [JsonPropertyName("id")]
    public int? Id { get; set; }

    [JsonPropertyName("inactive")]
    public bool? Inactive { get; set; }

    [JsonPropertyName("shopcode")]
    public string? Shopcode { get; set; }

    [JsonPropertyName("unit_price")]
    public int? UnitPrice { get; set; }

    [JsonPropertyName("combo")]
    public bool? Combo { get; set; }

    [JsonPropertyName("cost_price")]
    public double? CostPrice { get; set; }

    [JsonPropertyName("localization")]
    public string? Localization { get; set; }

    [JsonPropertyName("eangtin")]
    public string? Eangtin { get; set; }

    [JsonPropertyName("stock")]
    public int? Stock { get; set; }

    [JsonPropertyName("stockmin")]
    public int? Stockmin { get; set; }

    [JsonPropertyName("active")]
    public bool? Active { get; set; }

    [JsonPropertyName("combo_items")]
    public List<object> ComboItems { get; set; } = new();

    [JsonPropertyName("profit_margin")]
    public double? ProfitMargin { get; set; }

    [JsonPropertyName("can_change_price")]
    public bool? CanChangePrice { get; set; }

    [JsonPropertyName("auto_price")]
    public bool? AutoPrice { get; set; }

    [JsonPropertyName("price_promo")]
    public int? PricePromo { get; set; }

    [JsonPropertyName("published")]
    public bool? Published { get; set; }

    [JsonPropertyName("promotional")]
    public bool? Promotional { get; set; }

    [JsonPropertyName("image")]
    public Image Image { get; set; } = Image.Default;

    [JsonPropertyName("catalog_description")]
    public string? CatalogDescription { get; set; }

    [JsonPropertyName("has_stock")]
    public bool? HasStock { get; set; }

    [JsonPropertyName("current_stock")]
    public int? CurrentStock { get; set; }

    [JsonPropertyName("stockmax")]
    public int? Stockmax { get; set; }

    [JsonPropertyName("stock_control")]
    public bool? StockControl { get; set; }

    [JsonPropertyName("commission_percentage")]
    public int? CommissionPercentage { get; set; }

    [JsonPropertyName("commission_profit")]
    public bool? CommissionProfit { get; set; }

    [JsonPropertyName("default_markup")]
    public bool? DefaultMarkup { get; set; }

    [JsonPropertyName("net_weigth")]
    public int? NetWeigth { get; set; }

    [JsonPropertyName("gross_weigth")]
    public int? GrossWeigth { get; set; }

    [JsonPropertyName("subcategory")]
    public Subcategory Subcategory { get; set; } = new();

    [JsonPropertyName("reserved_stock")]
    public int? ReservedStock { get; set; }

    [JsonPropertyName("photos")]
    public List<object> Photos { get; set; } = new();

    [JsonPropertyName("suppliers")]
    public List<object> Suppliers { get; set; } = new();

    [JsonPropertyName("no_tax")]
    public bool? NoTax { get; set; }

    [JsonPropertyName("tax_use_global")]
    public bool? TaxUseGlobal { get; set; }

    [JsonPropertyName("cartUid")]
    public string? CartUid { get; set; }

    [JsonPropertyName("isPriceEdited")]
    public bool? IsPriceEdited { get; set; }
}

public class Seller
{
    public static Seller Default => new Seller()
    {
        Uid = "1255C06C-C3A5-470A-A0C1-205C230B53C7",
        Name = "MIGUEL OLIVEIRA",
        Username = "admin"
    };

    [JsonPropertyName("uid")]
    public string? Uid { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("username")]
    public string? Username { get; set; }
}

public class Subcategory
{
}

public class Tax
{
    public static Tax Default => new Tax()
    {
        Included = 0,
        ToInclude = 0,
        Exempt = true,
        ExemptValue = 0
    };

    [JsonPropertyName("included")]
    public int? Included { get; set; }

    [JsonPropertyName("to_include")]
    public int? ToInclude { get; set; }

    [JsonPropertyName("exempt")]
    public bool? Exempt { get; set; }

    [JsonPropertyName("exempt_value")]
    public int? ExemptValue { get; set; }
}

public class TaxDetails
{
}

public class Transporter
{
    public static Transporter Empty => new Transporter()
    {
        Brand = string.Empty,
        GrossWeight = 0,
        Id = 0,
        NetWeight = 0,
        Specie = string.Empty,
        Volume = 0,
        VolumetricWeight = 0
    };

    [JsonPropertyName("id")]
    public int? Id { get; set; }

    [JsonPropertyName("brand")]
    public string? Brand { get; set; }

    [JsonPropertyName("gross_weight")]
    public int? GrossWeight { get; set; }

    [JsonPropertyName("net_weight")]
    public int? NetWeight { get; set; }

    [JsonPropertyName("volumetric_weight")]
    public int? VolumetricWeight { get; set; }

    [JsonPropertyName("volume")]
    public int? Volume { get; set; }

    [JsonPropertyName("specie")]
    public string? Specie { get; set; }
}

public class Unit
{
}
