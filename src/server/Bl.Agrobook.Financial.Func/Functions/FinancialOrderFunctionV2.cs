using Bl.Agrobook.Financial.Func.Model;
using Bl.Agrobook.Financial.Func.Model.Kyte;
using Bl.Agrobook.Financial.Func.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Globalization;

namespace Bl.Agrobook.Financial.Func.Functions;

public class FinancialOrderFunctionV2
{
    private readonly AuthService _authService;
    private readonly ILogger<FinancialOrderFunctionV2> _logger;
    private readonly CsvOrderReader _csvOrderReader;
    private readonly FinancialKyteApiService _financialApiService;

    public FinancialOrderFunctionV2(
        AuthService authService,
        ILogger<FinancialOrderFunctionV2> logger,
        FinancialKyteApiService financialApiService,
        CsvOrderReader csvOrderReader)
    {
        _authService = authService;
        _logger = logger;
        _financialApiService = financialApiService;
        _csvOrderReader = csvOrderReader;
    }

    [Function("FinancialOrderV2")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "v2/financial/order/batch")] HttpRequest req,
        CancellationToken cancellationToken = default)
    {
        if (!_authService.IsAuthenticated(req))
        {
            return new UnauthorizedResult();
        }

        if (!req.HasFormContentType || req.Form.Files.Count == 0)
        {
            return new BadRequestObjectResult("No file uploaded.");
        }

        var file = req.Form.Files[0];

        try
        {
            var cultureInfoQuery = req.Query["cultureInfo"].ToString();

            if (string.IsNullOrWhiteSpace(cultureInfoQuery)) cultureInfoQuery = "pt-BR";

            CultureInfo cultureInfo = new(cultureInfoQuery);

            var creationModels = await CheckFileAsync(cultureInfo, file, cancellationToken);

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

            _logger.LogInformation("Customers: {customers}", customers.Count);

            // var currentOpenedOrders = // TODO: Check the current orders to avoid duplication

            var createModels = MapOrdersByInfo(creationModels, products, customers);

            var creationResult = new List<CreationOrderResultCsvModel>();

            foreach (var createModel in createModels)
            {
                //
                // Check if the order already exists for the customer
                // The check will be matched by the customer UID and the date of the order compared to the current date
                //
                var keyId = (createModel.Customer.Uid, DateOnly.FromDateTime(DateTime.Now));
                try
                {
                    // if (activeOrders.ContainsKey(keyId)) ;// TODO: Check the current orders before posting a new one

                    var result = await _financialApiService.CreateOrderAsync(
                        createModel.Customer, 
                        createModel.Products, 
                        cancellationToken);

                    creationResult.Add(new()
                    {
                        Status = "Ok",
                        Code = result["code"]?.ToString(),
                        Message = string.Empty,
                        Price = 0,
                        CustomerName = createModel.Customer.Name ?? string.Empty
                    });

                    await Task.Delay(200, cancellationToken);
                }
                catch (Exception e)
                {
                    creationResult.Add(new()
                    {
                        Status = "Falha",
                        Code = null,
                        Message = e.Message,
                        Price = 0,
                        CustomerName = createModel.Customer.Name ?? string.Empty
                    });
                }
            }

            return new OkObjectResult(creationResult);
        }
        catch (CsvHelper.HeaderValidationException e)
        {
            return new BadRequestObjectResult(new
            {
                Message = "Falha em validação de cabeçalho de CSV. Segue cabeçalhos que não puderam ser mapeados:\n"
                    + string.Join('\n', e.InvalidHeaders.Select(e => e.Names[0]))
            });
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occurred while processing the request.");
            return new BadRequestObjectResult(new
            {
                e.Message
            });
        }
    }

    private async Task<CreateOrderCsvModel[]> CheckFileAsync(
        CultureInfo cultureInfo,
        IFormFile formFile,
        CancellationToken cancellationToken = default)
    {
        using var fileStream = formFile.OpenReadStream();
        if (fileStream.CanSeek) fileStream.Seek(0, SeekOrigin.Begin);
        var orders = await _csvOrderReader.MapCreateOrderCsvAsync(fileStream, cultureInfo, cancellationToken);
        return orders;
    }

    private IEnumerable<(GetCustomerCustomerModel Customer, CreateCartProductModel[] Products)> MapOrdersByInfo(
        CreateOrderCsvModel[] models,
        GetProductsViewModel[] products,
        GetCustomerCustomerModel[] customers)
    {
        foreach (var model in models)
        {

        }
    }
}