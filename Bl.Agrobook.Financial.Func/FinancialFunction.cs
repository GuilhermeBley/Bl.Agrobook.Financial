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

        var file = req.Form.Files[0];
        using var fileStream = file.OpenReadStream();
        var orders = await _csvOrderReader.MapCreateOrderCsvAsync(fileStream, cancellationToken);
        var ordersToCreate = orders.ToDictionary(o => o.CustomerCode, o =>
        {

        });

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

            return new OkObjectResult("Welcome to Azure Functions!");
        }
        catch(Exception e)
        {
            _logger.LogError(e, "An error occurred while processing the request.");
            return new BadRequestObjectResult($"Failed to execute.");
        }
    }
}
