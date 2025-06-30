using Bl.Agrobook.Financial.Func.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Bl.Agrobook.Financial.Func.Functions;

public class DeliveryDatesFinancialFunc
{
    private readonly AuthService _authService;
    private readonly ILogger<FinancialOrderFunction> _logger;
    private readonly PreOrderService _preOrderService;

    public DeliveryDatesFinancialFunc(
        AuthService authService,
        ILogger<FinancialOrderFunction> logger,
        PreOrderService preOrderService)
    {
        _authService = authService;
        _logger = logger;
        _preOrderService = preOrderService;
    }

    [Function("DeliveryDatesFinancial")]
    public async Task<IActionResult> RunFunc(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", "get", Route = "financial/delivery-dates")] HttpRequest req,
        CancellationToken cancellationToken = default)
    {
        try
        {
            if (req.Method.Equals("GET", StringComparison.OrdinalIgnoreCase))
            {
                return await RunFinancialDeliveryDateQuery(req, cancellationToken);
            }
            else if (req.Method.Equals("POST", StringComparison.OrdinalIgnoreCase))
            {
                return await RunFinancialDeliveryDateCreation(req, cancellationToken);
            }

            return new BadRequestObjectResult("Only GET and POST methods are supported");
        }
        catch (ApiException e)
        {
            return new BadRequestObjectResult(new
            {
                ErrorResponses = e.ErrorResponses
            });
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to insert delivery date.");
            return new BadRequestObjectResult(
                new { Message = "Erro ao inserir data de coleta." });
        }
    }

    private async Task<IActionResult> RunFinancialDeliveryDateQuery(HttpRequest req, CancellationToken cancellationToken = default)
    {
        var dts = await _preOrderService.GetDeliveryDatesAsync(cancellationToken);

        if (dts is null || !dts.Any())
        {
            return new NoContentResult();
        }

        return new OkObjectResult(dts);
    }

    private async Task<IActionResult> RunFinancialDeliveryDateCreation(HttpRequest req, CancellationToken cancellationToken = default)
    {
        if (!_authService.IsAuthenticated(req)) return new UnauthorizedResult();

        var node = await JsonSerializer.DeserializeAsync<JsonNode>(req.Body);

        if (node is null || !DateOnly.TryParseExact(node["deliveryDate"]?.ToString(), "yyyy-MM-dd", out var dd))
        {
            return new BadRequestObjectResult("Invalid request body.");
        }

        await _preOrderService.CreateDeliveryDateAsync(dd, "api@email.com", cancellationToken);

        return new CreatedResult("api/financial/delivery-dates", new { DeliveryDate = dd });
    }
}