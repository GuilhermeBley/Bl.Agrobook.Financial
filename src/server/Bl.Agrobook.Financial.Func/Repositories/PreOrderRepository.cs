using Bl.Agrobook.Financial.Func.Model;
using Bl.Agrobook.Financial.Func.Options;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace Bl.Agrobook.Financial.Func.Repositories;

public class PreOrderRepository : RepositoryBase
{
    private const string CollectionName = "PreOrder";

    public PreOrderRepository(IOptions<MongoDbOptions> options) : base(options)
    {
    }

    public async Task InsertAsync(PreOrderModel model, CancellationToken cancellationToken = default)
    {
        var collection = GetCollection<PreOrderModel>(CollectionName);
        await collection.InsertOneAsync(model, new() { }, cancellationToken);
    }

    public async Task InsertManyAsync(IEnumerable<PreOrderModel> model, CancellationToken cancellationToken = default)
    {
        var collection = GetCollection<PreOrderModel>(CollectionName);
        await collection.InsertManyAsync(model, new() { }, cancellationToken);
    }

    public async Task<PreOrderModel> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var collection = GetCollection<PreOrderModel>(CollectionName);
        var filter = Builders<PreOrderModel>.Filter.Eq(p => p.Id, id);
        return await collection.Find(filter).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<PreOrderModel>> GetByDeliveryDateAsync(DateOnly deliveryDate, CancellationToken cancellationToken = default)
    {
        var collection = GetCollection<PreOrderModel>(CollectionName);
        var filter = Builders<PreOrderModel>.Filter.Eq(p => p.DeliveryAt, deliveryDate);
        return await collection.Find(filter).ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<PreOrderModel>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var collection = GetCollection<PreOrderModel>(CollectionName);
        return await collection.Find(_ => true).ToListAsync(cancellationToken);
    }

    public async Task<bool> UpdateAsync(PreOrderModel preOrder, CancellationToken cancellationToken = default)
    {
        var collection = GetCollection<PreOrderModel>(CollectionName);
        var filter = Builders<PreOrderModel>.Filter.Eq(p => p.Id, preOrder.Id);
        var result = await collection.ReplaceOneAsync(filter, preOrder, cancellationToken: cancellationToken);
        return result.IsAcknowledged && result.ModifiedCount > 0;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var collection = GetCollection<PreOrderModel>(CollectionName);
        var filter = Builders<PreOrderModel>.Filter.Eq(p => p.Id, id);
        var result = await collection.DeleteOneAsync(filter, cancellationToken);
        return result.IsAcknowledged && result.DeletedCount > 0;
    }
}
