using Bl.Agrobook.Financial.Func.Model;
using Bl.Agrobook.Financial.Func.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Globalization;

namespace Bl.Agrobook.Financial.Func.Functions;

public class FinancialOrderFunction
{
    private readonly AuthService _authService;
    private readonly ILogger<FinancialOrderFunction> _logger;
    private readonly FinancialApiService _financialApiService;
    private readonly SalesService _salesService;

    public FinancialOrderFunction(
        AuthService authService,
        ILogger<FinancialOrderFunction> logger,
        FinancialApiService financialApiService,
        CsvOrderReader csvOrderReader,
        SalesService salesService)
    {
        _authService = authService;
        _logger = logger;
        _financialApiService = financialApiService;
        _salesService = salesService;
    }

    [Function("FinancialOrder")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "financial/order/batch")] HttpRequest req,
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

            var ordersToCreate = await _salesService.CheckFileAsync(cultureInfo, file, cancellationToken);

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

            var currentOpenedOrders = await _financialApiService.GetOrdersAsync(alreadyOpen: true).ToListAsync(cancellationToken);

            _logger.LogInformation("curentOpenedOrders: {customers}", currentOpenedOrders.Count);

            var createModels = _salesService.MapOrdersByFormFileAsync(cultureInfo, ordersToCreate, products, customers, cancellationToken);

            var creationResult = new List<CreationOrderResultCsvModel>();

            var activeOrders = currentOpenedOrders
                .Where(e => e.Date is not null)
                .Select(e => new { 
                    Key = (e.Customer.Uid, DateOnly.FromDateTime(e.Date!.Value)),
                    Value = e
                })
                .DistinctBy(e => e.Key)
                .ToDictionary(e => e.Key, e => e.Value);

            foreach (var createModel in createModels)
            {
                //
                // Check if the order already exists for the customer
                // The check will be matched by the customer UID and the date of the order compared to the current date
                //
                var keyId = (createModel.Customer.Uid, DateOnly.FromDateTime(DateTime.Now));
                try
                {
                    if (activeOrders.ContainsKey(keyId))
                    {
                        var orderAlreadyAdded = activeOrders[keyId];
                        creationResult.Add(new()
                        {
                            Status = "Ok",
                            Code = orderAlreadyAdded?.Code,
                            Message = $"Nenhum novo pedido criado, pedido {orderAlreadyAdded?.Code} já aberto para cliente {createModel.Customer.Name}.",
                            Price = createModel.FinalValue ?? 0,
                            CustomerName = createModel.Customer.Name ?? string.Empty
                        });
                        continue;
                    }

                    var result = await _financialApiService.CreateOrderAsync(createModel, cancellationToken);

                    creationResult.Add(new()
                    {
                        Status = "Ok",
                        Code = result["code"]?.ToString(),
                        Message = string.Empty,
                        Price = createModel.NetValue ?? 0,
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
                        Price = createModel.NetValue!.Value,
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
        catch(Exception e)
        {
            _logger.LogError(e, "An error occurred while processing the request.");
            return new BadRequestObjectResult(new
            {
                e.Message
            });
        }
    }
}
