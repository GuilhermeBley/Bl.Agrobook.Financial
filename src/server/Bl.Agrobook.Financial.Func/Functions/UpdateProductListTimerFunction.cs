using System;
using System.Threading.Tasks;
using Bl.Agrobook.Financial.Func.Model;
using Bl.Agrobook.Financial.Func.Repositories;
using Bl.Agrobook.Financial.Func.Services;
using iText.Commons.Utils;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Bl.Agrobook.Financial.Func.Functions;

public class UpdateProductListTimerFunction
{
#if DEBUG
    public const bool ExecuteOnStartup = false;
#else
    public const bool ExecuteOnStartup = false;
#endif

    private readonly ILogger _logger;
    private readonly FinancialApiService _financialApiService;
    private readonly ProductRepository _productRepository;

    public UpdateProductListTimerFunction(
        ILoggerFactory loggerFactory,
        FinancialApiService financialApiService,
        ProductRepository productRepository)
    {
        _logger = loggerFactory.CreateLogger<UpdateProductListTimerFunction>();
        _financialApiService = financialApiService;
        _productRepository = productRepository;

    }

    [Function("UpdateProductListTimerFunction")]
    public async Task Run(
        [TimerTrigger("0 */5 * * * *", RunOnStartup = ExecuteOnStartup)] /*Update for each five minutes*/ TimerInfo myTimer,
        CancellationToken cancellationToken = default)
    {
        var start = DateTime.Now;

        try
        {
            await foreach (var p in _financialApiService.GetProductsAsync(cancellationToken))
            {
                try
                {
                    await AddOrUpdateProductAsync(p, cancellationToken);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, $"Error occurred while processing product {p.Code}.");
                    continue;
                }
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occurred while updating product list.");
        }
        finally
        {
            _logger.LogInformation($"UpdateProductList Timer trigger function ended up in {(DateTime.Now - start).TotalSeconds} seconds.");
        }
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
