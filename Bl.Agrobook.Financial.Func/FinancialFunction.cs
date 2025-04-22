using Bl.Agrobook.Financial.Func.Model;
using Bl.Agrobook.Financial.Func.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Bl.Agrobook.Financial.Func;

public class FinancialFunction
{
    private readonly ILogger<FinancialFunction> _logger;
    private readonly CsvOrderReader _csvOrderReader;
    private readonly FinancialApiService _financialApiService;

    public FinancialFunction(
        ILogger<FinancialFunction> logger,
        FinancialApiService financialApiService,
        CsvOrderReader csvOrderReader)
    {
        _logger = logger;
        _financialApiService = financialApiService;
        _csvOrderReader = csvOrderReader;
    }

    [Function("FinancialOrder")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "financial/order/batch")] HttpRequest req,
        CancellationToken cancellationToken = default)
    {
        if (!req.Headers.TryGetValue("x-api-key", out var apiKey) ||
            apiKey != Environment.GetEnvironmentVariable("ExpectedApiKey"))
        {
            return new UnauthorizedResult();
        }

        if (req.Form.Files.Count == 0)
        {
            return new BadRequestObjectResult("No file uploaded.");
        }

        var file = req.Form.Files[0];

        try
        {
            var products = await _financialApiService.GetProductsAsync().ToListAsync(cancellationToken);

            if (products.Count == 0)
            {
                return new NotFoundObjectResult("No products found.");
            }

            _logger.LogInformation("Products: {Products}", products.Count);

            var customers = await _financialApiService.GetCustomersAsync().ToListAsync(cancellationToken);

            if (customers.Count == 0)
            {
                return new NotFoundObjectResult("No customers found.");
            }

            _logger.LogInformation("Customers: {customers}", products.Count);

            var createModels = await MapOrdersByFormFileAsync(file, products, customers, cancellationToken);

            var creationResult = new List<CreationOrderResultCsvModel>();

            foreach (var createModel in createModels)
            {
                try
                {
                    var result = await _financialApiService.CreateOrderAsync(createModel, cancellationToken);

                    creationResult.Add(new()
                    {
                        Status = "Ok",
                        Code = result["code"]?.ToString(),
                        Message = string.Empty,
                        Price = createModel.NetValue!.Value,
                        CustomerName = createModel.Customer.Name ?? string.Empty
                    });
                }
                catch (Exception e)
                {
                    creationResult.Add(new()
                    {
                        Status = "Falha",
                        Code = null,
                        Message = e.Message,
                        Price = createModel.NetValue!.Value,
                        CustomerName = createModel.Customer.Name ?? string.Empty
                    });
                } 
            }

            return new OkObjectResult(creationResult);
        }
        catch(Exception e)
        {
            _logger.LogError(e, "An error occurred while processing the request.");
            return new BadRequestObjectResult(new
            {
                Message = e.Message
            });
        }
    }

    private async Task<CreateCustomerOrderViewModel[]> MapOrdersByFormFileAsync(
        IFormFile formFile,
        IReadOnlyList<ProductViewModel> products,
        IReadOnlyList<CustomerViewModel> customers,
        CancellationToken cancellationToken = default)
    {
        using var fileStream = formFile.OpenReadStream();
        var orders = await _csvOrderReader.MapCreateOrderCsvAsync(fileStream, cancellationToken);
        Dictionary<int, CreateCustomerOrderViewModel> ordersByCustomer = new();

        for (var i = 0; i <= orders.Length; i++)
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
                        Seller = Seller.Default,
                        Shopcode = customer.Shopcode,
                        Status = 3,
                        Tax = Tax.Default,
                        Transporter = Transporter.Empty,
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
                o.Products.Add(new()
                {
                    Uid = product.Uid,
                    Tax = Tax.Default,
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
                    Id = 0,
                    Image = Image.Default,
                    ImageUrl = string.Empty,
                    Inactive = false,
                    IsPriceEdited = true, // use the price from the CSV
                    ItemDiscount = 0,
                    Localization = string.Empty,
                    NetWeigth = 0,
                    NoTax = false,
                    Obs = string.Empty,
                    Payd = 0,
                    PercentageDiscount = 0,
                    PercentualSale = false,
                    Photos = new(),
                    PricePromo = 0,
                    ProfitMargin = 0,
                    Promotional = false,
                    ProratedDiscount = 0,
                    Published = false,
                    Qty = order.Quantity,
                    ReservedStock = product.ReservedStock,
                    Stock = product.Stock,
                    StockControl = product.StockControl,
                    Stockmax = 0,
                    Stockmin = 0,
                    Subcategory = product.Subcategory,
                    Suppliers = new(),
                    TaxDetails = new(),
                    TaxUseGlobal = false,
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
                var csvRow = i + 1;
                _logger.LogError(e, "Error processing order {i}", csvRow);
                throw new Exception($"Falha ao processar linha {csvRow}. " + e.Message);
            }
        }

        return ordersByCustomer.Values.ToArray();
    }
}
