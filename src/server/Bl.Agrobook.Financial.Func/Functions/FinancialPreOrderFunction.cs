using Bl.Agrobook.Financial.Func.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Bl.Agrobook.Financial.Func.Functions;

public class FinancialPreOrderFunction
{
    private readonly AuthService _authService;
    private readonly ILogger<FinancialOrderFunction> _logger;
    private readonly PreOrderService _preOrderService;
    private readonly FinancialApiService _financialApiService;

    public FinancialPreOrderFunction(
        AuthService authService,
        ILogger<FinancialOrderFunction> logger,
        FinancialApiService financialApiService,
        PreOrderService preOrderService)
    {
        _authService = authService;
        _logger = logger;
        _financialApiService = financialApiService;
        _preOrderService = preOrderService;
    }

    [Function("FinancialPreOrderCreation")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "financial/preorder")] HttpRequest req,
        CancellationToken cancellationToken = default)
    {

    }
}
