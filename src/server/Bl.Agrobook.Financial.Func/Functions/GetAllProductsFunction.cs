using Bl.Agrobook.Financial.Func.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Bl.Agrobook.Financial.Func.Functions;

public class GetAllProductsFunction
{
    private readonly ILogger<GetAllProductsFunction> _logger;
    private readonly ProductRepository _productRepository;

    public GetAllProductsFunction(
        ILogger<GetAllProductsFunction> logger,
        ProductRepository productRepository)
    {
        _logger = logger;
        _productRepository = productRepository;
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
}
