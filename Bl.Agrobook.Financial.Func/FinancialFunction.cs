using Bl.Agrobook.Financial.Func.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Bl.Agrobook.Financial.Func;

public class FinancialFunction
{
    private readonly ILogger<FinancialFunction> _logger;
    private readonly FinancialApiService _financialApiService;

    public FinancialFunction(
        ILogger<FinancialFunction> logger,
        FinancialApiService financialApiService)
    {
        _logger = logger;
        _financialApiService = financialApiService;
    }

    [Function("Function1")]
    public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
    {
        
        _financialApiService.

        return new OkObjectResult("Welcome to Azure Functions!");
    }
}
