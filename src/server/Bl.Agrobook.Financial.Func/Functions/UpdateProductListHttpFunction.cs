using System;
using System.Threading.Tasks;
using Bl.Agrobook.Financial.Func.Model;
using Bl.Agrobook.Financial.Func.Repositories;
using Bl.Agrobook.Financial.Func.Services;
using iText.Commons.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Bl.Agrobook.Financial.Func.Functions;

public class UpdateProductListHttpFunction
{
    private readonly AuthService _authService;
    private readonly ILogger _logger;
    private readonly FinancialApiService _financialApiService;
    private readonly ProductRepository _productRepository;

    public UpdateProductListHttpFunction(
        ILoggerFactory loggerFactory,
        AuthService authService,
        FinancialApiService financialApiService,
        ProductRepository productRepository)
    {
        _authService = authService;
        _logger = loggerFactory.CreateLogger<UpdateProductListTimerFunction>();
        _financialApiService = financialApiService;
        _productRepository = productRepository;

    }

    [Function("UpdateProductListHttpFunction")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "products/update-all")] HttpRequest req,
        CancellationToken cancellationToken = default)
    {
        if (!_authService.IsAuthenticated(req))
        {
            return new UnauthorizedResult();
        }

        var start = DateTime.Now;

        try
        {
            await foreach (var p in _financialApiService.GetProductsAsync(cancellationToken))
            {
                try
                {
                    await AddOrUpdateProductAsync(p, cancellationToken);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception e)
                {
                    _logger.LogError(e, $"Error occurred while processing product {p.Code}.");
                    continue;
                }
            }
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("Operation was cancelled while updating product list.");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occurred while updating product list.");
            return new BadRequestObjectResult(new
            {
                Message = "Falha em executar operações."
            });
        }
        finally
        {
            _logger.LogInformation($"UpdateProductList function ended up in {(DateTime.Now - start).TotalSeconds} seconds.");
        }

        return new OkResult();
    }

    private async Task AddOrUpdateProductAsync(ProductViewModel p, CancellationToken cancellationToken = default)
    {
        var product = await _productRepository.GetProductByIdAsync(p.Code, cancellationToken);

        var productExists = product is not null;

        product ??= new ProductModel()
        {
            Code = p.Code,
            AvailableQuantity = p.Stock,
            Active = p.Active ?? false,
            Description = p.CatalogDescription,
            ImgUrl = p.Image.Url,
            Name = p.Description ?? string.Empty,
            Price = p.Price,
            InsertedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow,
        };

        if (productExists)
        {
            product.Active = p.Active ?? false;
            product.AvailableQuantity = p.Stock;
            product.Description = p.CatalogDescription;
            product.ImgUrl = p.Image.Url;
            product.Name = p.Description ?? string.Empty;
            product.Price = p.Price;
            product.UpdatedAt = DateTimeOffset.UtcNow;
            product.InsertedAt = product.InsertedAt < DateTimeOffset .UtcNow.AddYears(-100) ? DateTimeOffset.UtcNow : product.InsertedAt;
            await _productRepository.UpdateProductAsync(product, cancellationToken);
            _logger.LogInformation($"Product {product.Code} updated.");
        }
        else
        {
            await _productRepository.InsertProductAsync(product, cancellationToken);
            _logger.LogInformation($"Product {product.Code} inserted.");
        }
    }
}
