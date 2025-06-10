using Bl.Agrobook.Financial.Func.Json;
using Bl.Agrobook.Financial.Func.Repositories;
using Bl.Agrobook.Financial.Func.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using System.Text.Json;

namespace Bl.Agrobook.Financial.Func.Functions;

public class FinancialPreOrderFunction
{
    private readonly AuthService _authService;
    private readonly ILogger<FinancialOrderFunction> _logger;
    private readonly PreOrderService _preOrderService;

    public FinancialPreOrderFunction(
        AuthService authService,
        ILogger<FinancialOrderFunction> logger,
        PreOrderService preOrderService)
    {
        _authService = authService;
        _logger = logger;
        _preOrderService = preOrderService;
    }

    [Function("FinancialPreOrderCreation")]
    public async Task<IActionResult> RunFinancialPreOrderCreation(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "financial/preorder")] HttpRequest req,
        CancellationToken cancellationToken = default)
    {

        try
        {
            var model = await JsonSerializer.DeserializeAsync<Model.CreatePreOrderModel>(req.Body, new JsonSerializerOptions
            {
                Converters = { new DateOnlyJsonConverter() }
            });

            ArgumentNullException.ThrowIfNull(model, nameof(model));

            var resp = await _preOrderService.InsertPreOrderAsync(model);

            return new CreatedResult(
                $"/api/financial/preorder/{resp.Id}",
                new { Id = resp.Id, Message = "Pré-pedido inserido com sucesso." });
        }
        catch(Exception e)
        {
            _logger.LogError(e, "Failed to insert pre-order.");
            return new BadRequestObjectResult(
                new { Message = "Erro ao inserir pré-pedido." });
        }
    }

    [Function("FinancialPreOrderQuery")]
    public async Task<IActionResult> RunFinancialPreOrderQuery(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "financial/preorder")] HttpRequest req,
        CancellationToken cancellationToken = default)
    {
        if (!_authService.IsAuthenticated(req))
        {
            return new UnauthorizedResult();
        }

        try
        {
            var deliveryDate = req.Query["deliveryDate"].ToString();
            if (deliveryDate is null || 
                DateOnly.TryParseExact(deliveryDate, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out var dt))
                return new BadRequestObjectResult(
                    new { Message = "Erro ao consultar. Parâmetros inválidos." });

            var resp = await _preOrderService.GetByDeliveryDateAsync(dt, cancellationToken);

            return new OkObjectResult(resp);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to get pre-orders.");
            return new BadRequestObjectResult(
                new { Message = "Erro ao consultar." });
        }
    }
}
