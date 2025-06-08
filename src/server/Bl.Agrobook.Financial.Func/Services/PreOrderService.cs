using Bl.Agrobook.Financial.Func.Model;
using Bl.Agrobook.Financial.Func.Repositories;

namespace Bl.Agrobook.Financial.Func.Services;

public class PreOrderService
{
    private readonly PreOrderRepository _preOrderRepository;

    public PreOrderService(PreOrderRepository preOrderRepository)
    {
        _preOrderRepository = preOrderRepository;
    }

    public async Task InsertPreOrderAsync(CreatePreOrderModel preOrderRequest, CancellationToken cancellationToken = default)
    {
        var result = ModelValidator.Validate(preOrderRequest);

        if (result.Any())
        {
            throw new ArgumentException("Dados inválidos.");
        }


        await _preOrderRepository.InsertPreOrderAsync(
            new PreOrderModel()
            {
                CustomerCode = null,
                CustomerEmail = preOrderRequest.CustomerEmail.Trim(),
                CustomerName = preOrderRequest.CustomerName.Trim(),
                CustomerPhone = string.Concat(preOrderRequest.CustomerPhone.Where(char.IsNumber)),
                DeliveryAt = preOrderRequest.DeliveryAt,
                Id = Guid.NewGuid(),
                InsertedAt = DateTime.UtcNow,
                Obs = preOrderRequest.Obs,
                OrderCode = null,
                Product = preOrderRequest.Product.Select(p => new PreOrderProductModel()
                {
                    Id = Guid.NewGuid(),
                    ProductCode = p.ProductCode.Trim(),
                    ProductName = p.ProductName.Trim(),
                    Quantity = p.Quantity
                }).ToList(),
                UpdateAt = DateTime.Now
            }, 
            cancellationToken);
    }
}
