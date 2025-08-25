using Bl.Agrobook.Financial.Func.Repositories;
using Bl.Agrobook.Financial.Func.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Bl.Agrobook.Financial.Func.Functions;

public class GetAllProductsFunction
{
    private readonly ILogger<GetAllProductsFunction> _logger;
    private readonly ProductRepository _productRepository;
    private readonly FinancialKyteApiService _kyteApi;

    public GetAllProductsFunction(
        ILogger<GetAllProductsFunction> logger,
        ProductRepository productRepository,
        FinancialKyteApiService kyteApi)
    {
        _logger = logger;
        _productRepository = productRepository;
        _kyteApi = kyteApi;
    }

    [Function("GetAllProductsFunction")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "product")] HttpRequest req,
        CancellationToken cancellationToken = default)
    {
        var products = await _productRepository.GetAllProductsAsync(cancellationToken);

        if (products == null || !products.Any())
        {
            return new NoContentResult();
        }

        return new OkObjectResult(products);
    }

    [Function("GetAllProductsFunctionV2")]
    public async Task<IActionResult> RunV2(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "v2/product")] HttpRequest req,
        CancellationToken cancellationToken = default)
    {
        var products = await _kyteApi.GetProductsAsync().ToListAsync(cancellationToken);

        if (products == null || !products.Any())
        {
            return new NoContentResult();
        }

        return new OkObjectResult(products);
    }
}
