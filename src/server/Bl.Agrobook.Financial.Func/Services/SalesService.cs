using Bl.Agrobook.Financial.Func.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Globalization;

namespace Bl.Agrobook.Financial.Func.Services;
public class SalesService
{
    public readonly ILogger<SalesService> _logger;
    private readonly CsvOrderReader _csvOrderReader;

    public SalesService(ILogger<SalesService> logger, CsvOrderReader csvOrderReader)
    {
        _logger = logger;
        _csvOrderReader = csvOrderReader;
    }

    public async Task<CreateOrderCsvModel[]> CheckFileAsync(
        CultureInfo cultureInfo,
        IFormFile formFile,
        CancellationToken cancellationToken = default)
    {
        using var fileStream = formFile.OpenReadStream();
        if (fileStream.CanSeek) fileStream.Seek(0, SeekOrigin.Begin);
        var orders = await _csvOrderReader.MapCreateOrderCsvAsync(fileStream, cultureInfo, cancellationToken);
        return orders;
    }

    public CreateCustomerOrderViewModel[] MapOrdersByFormFileAsync(
        CultureInfo cultureInfo,
        CreateOrderCsvModel[] orders,
        IReadOnlyList<ProductViewModel> products,
        IReadOnlyList<CustomerViewModel> customers,
        CancellationToken cancellationToken = default)
    {
        Dictionary<string, CreateCustomerOrderViewModel> ordersByCustomer = new();

        for (var i = 0; i < orders.Length; i++)
        {
            try
            {
                var order = orders[i];
                if (!ordersByCustomer.ContainsKey(order.CustomerCode))
                {
                    var customer = customers.FirstOrDefault(c => c.Code == order.CustomerCode.ToString());

                    ArgumentNullException.ThrowIfNull(customer, $"Customer with code {order.CustomerCode} not found.");

                    ordersByCustomer[order.CustomerCode] = new()
                    {
                        Canceled = false,
                        Id = 0,
                        Change = 0,
                        IncludedOn = DateTime.UtcNow,
                        Credit = 0,
                        CreditValue = 0,
                        Debt = 0,
                        Deliver = false,
                        DeliveryTax = 0,
                        Devolution = false,
                        Discount = 0,
                        FinalValue = 0,
                        GrossValue = 0,
                        ItemsDiscount = 0,
                        NetValue = 0,
                        Obs = string.Empty,
                        Origin = 3,
                        Payd = 0,
                        Payments = new(),
                        PendingPayment = true,
                        PercentageDiscount = 0,
                        PercentualSale = false,
                        Products = new(),
                        ProratedDiscount = 0,
                        Qty = 0,
                        Seller = SellerViewModel.Default,
                        Shopcode = customer.Shopcode,
                        Status = 1,
                        Tax = TaxViewModel.Default,
                        Transporter = TransporterViewModel.Empty,
                        Uid = Guid.NewGuid().ToString(),
                        UsedCredit = 0,
                        Customer = new()
                        {
                            Address = customer.Addresses.FirstOrDefault() ?? new(),
                            Ddi = customer.Ddi,
                            Document = customer.CpfCnpj,
                            Email = customer.Email,
                            Name = customer.Name,
                            Uid = customer.Uid,
                            Phone = customer.Phone,
                        }
                    };
                }

                var product = products.FirstOrDefault(p => p.Code == order.ProductCode.ToString());

                ArgumentNullException.ThrowIfNull(product, $"Product with code {order.ProductCode} not found.");

                var o = ordersByCustomer[order.CustomerCode];

                if (!string.IsNullOrWhiteSpace(order.ObsPedido))
                {
                    var obss = new string?[] { o.Obs?.Trim('.'), order.ObsPedido.Trim(' ', '.', ',', '\n') + "." };
                    o.Obs = string.Join(", ", obss.Where(e => !string.IsNullOrEmpty(e)));
                }

                if (string.IsNullOrWhiteSpace(order.ObsProduto))
                {
                    order.ObsProduto = string.Empty;
                }

                o.Products.Add(new()
                {
                    Uid = product.Uid,
                    Tax = TaxViewModel.Default,
                    Shopcode = product.Shopcode,
                    Active = true,
                    AutoPrice = false,
                    Brand = product.Brand,
                    CanChangePrice = true,
                    CartUid = Guid.NewGuid().ToString(),
                    CatalogDescription = product.CatalogDescription,
                    Category = product.Category,
                    Code = product.Code,
                    CodeExtra = null,
                    Combo = false,
                    ComboChild = false,
                    ComboItems = [],
                    ComboParent = false,
                    CommissionPercentage = 0,
                    CommissionProfit = false,
                    CurrentStock = product.CurrentStock,
                    Debt = 0,
                    Description = product.Description,
                    DefaultMarkup = product.DefaultMarkup,
                    Discount = 0,
                    Eangtin = product.Eangtin,
                    EanGtin = product.Eangtin,
                    ExtraCode = product.ExtraCode,
                    FractionalSale = false,
                    GrossWeigth = 0,
                    HasStock = product.HasStock,
                    Id = product.Id,
                    Image = ImageViewModel.Default,
                    ImageUrl = string.Empty,
                    Inactive = false,
                    IsPriceEdited = true, // use the price from the CSV
                    ItemDiscount = 0,
                    Localization = string.Empty,
                    NetWeigth = 0,
                    NoTax = false,
                    Obs = order.ObsProduto,
                    Payd = 0,
                    PercentageDiscount = 0,
                    PercentualSale = false,
                    Photos = new(),
                    PricePromo = 0,
                    ProfitMargin = 0,
                    Promotional = false,
                    ProratedDiscount = 0,
                    Published = true,
                    Qty = order.Quantity,
                    ReservedStock = product.ReservedStock,
                    Stock = product.Stock,
                    StockControl = product.StockControl,
                    Stockmax = 0,
                    Stockmin = 0,
                    Subcategory = product.Subcategory,
                    Suppliers = new(),
                    TaxDetails = new(),
                    TaxUseGlobal = true,
                    Unit = product.Unit,
                    UnitPrice = 0,

                    CostPrice = order.Price,
                    Price = order.Price,
                    FinalValue = order.Price * order.Quantity,
                    GrossValue = order.Price * order.Quantity,
                    NetValue = order.Price * order.Quantity,
                });
                o.FinalValue += order.Price * order.Quantity;
                o.GrossValue = o.FinalValue;
                o.NetValue = o.FinalValue;
            }
            catch (Exception e)
            {
                var csvDataRow = i + 1/*index*/ + 1 /*CSV header*/;
                _logger.LogError(e, "Error processing order {i}", csvDataRow);
                throw new Exception($"Falha ao processar linha {csvDataRow}. " + e.Message, e);
            }
        }

        return ordersByCustomer.Values.ToArray();
    }
}
