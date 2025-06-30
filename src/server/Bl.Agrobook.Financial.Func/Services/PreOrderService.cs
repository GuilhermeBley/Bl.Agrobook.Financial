using Bl.Agrobook.Financial.Func.Model;
using Bl.Agrobook.Financial.Func.Repositories;

namespace Bl.Agrobook.Financial.Func.Services;

public record PreOrderCreatedResult(Guid Id);

public class PreOrderService
{
    private readonly PreOrderRepository _preOrderRepository;
    private readonly DeliveryDateRepository _deliveryDateRepository;

    public PreOrderService(PreOrderRepository preOrderRepository, DeliveryDateRepository deliveryDateRepository)
    {
        _preOrderRepository = preOrderRepository;
        _deliveryDateRepository = deliveryDateRepository;
    }

    public async Task<PreOrderCreatedResult> InsertPreOrderAsync(CreatePreOrderModel preOrderRequest, CancellationToken cancellationToken = default)
    {
        var result = ModelValidator.Validate(preOrderRequest);

        if (result.Any())
        {
            throw new ApiException(result);
        }

        var deliveryId = DeliveryDateModel.GenerateId(preOrderRequest.DeliveryAt);
        if ((await _deliveryDateRepository.GetByIdAsync(deliveryId, cancellationToken)) is null)
        {
            throw new ApiException(
                ApiErrorResult<CreatePreOrderModel>.Create(
                    $"Data de entrega de produtos indisponíveis para dia {preOrderRequest.DeliveryAt:dd/MM/yyyy}.",
                    p => p.DeliveryAt));
        }

        var model =
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
                Product = preOrderRequest.Products.Select(p => new PreOrderProductModel()
                {
                    Id = Guid.NewGuid(),
                    ProductCode = p.ProductCode.Trim(),
                    ProductName = p.ProductName.Trim(),
                    Quantity = p.Quantity
                }).ToList(),
                UpdateAt = DateTime.Now
            };
        await _preOrderRepository.InsertAsync(model, cancellationToken);

        return new(model.Id);
    }

    public async Task<IEnumerable<PreOrderModel>> GetByDeliveryDateAsync(
        DateOnly deliveryDate,
        CancellationToken cancellationToken = default)
    {
        var orders = await _preOrderRepository.GetByDeliveryDateAsync(deliveryDate, cancellationToken);

        return orders;
    }

    public async Task<DateOnly[]> GetDeliveryDatesAsync(CancellationToken cancellationToken = default)
    {
        var deliveryDates = await _deliveryDateRepository.GetAllGreatherThanAsync(
            DateOnly.FromDateTime(DateTime.Now),
            cancellationToken);

        return deliveryDates.Select(x => x.DeliveryAt).ToArray();
    }

    public async Task CreateDeliveryDateAsync(
        DateOnly deliveryDate,
        string userEmail,
        CancellationToken cancellationToken = default)
    {
        var id = DeliveryDateModel.GenerateId(deliveryDate);
        var alreadyAdded = await _deliveryDateRepository.GetByIdAsync(id);
        if (alreadyAdded is not null) return;

        await _deliveryDateRepository.InsertAsync(new DeliveryDateModel
        {
            Id = id,
            DeliveryAt = deliveryDate,
            InsertedAt = DateTime.UtcNow,
            UserId = userEmail.Trim(),
        },cancellationToken);
    }
}
