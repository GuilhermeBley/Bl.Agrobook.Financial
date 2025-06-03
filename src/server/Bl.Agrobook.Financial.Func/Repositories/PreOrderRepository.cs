using Bl.Agrobook.Financial.Func.Model;
using Bl.Agrobook.Financial.Func.Options;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Bl.Agrobook.Financial.Func.Repositories;

public class PreOrderRepository : RepositoryBase
{
    private const string CollectionName = "PreOrder";

    public PreOrderRepository(IOptions<MongoDbOptions> options) : base(options)
    {
    }

    public async Task InsertProductAsync(PreOrderModel preOrder, CancellationToken cancellationToken = default)
    {
        var collection = GetCollection<PreOrderModel>(CollectionName);
        await collection.InsertOneAsync(preOrder, new() { }, cancellationToken);
    }

    public async Task InsertManyProductsAsync(IEnumerable<PreOrderModel> preOrders, CancellationToken cancellationToken = default)
    {
        var collection = GetCollection<PreOrderModel>(CollectionName);
        await collection.InsertManyAsync(preOrders, new() { }, cancellationToken);
    }

    public async Task<PreOrderModel> GetProductByIdAsync(string preOrderCode, CancellationToken cancellationToken = default)
    {
        var collection = GetCollection<PreOrderModel>(CollectionName);
        var filter = Builders<PreOrderModel>.Filter.Eq(p => p.Code, preOrderCode);
        return await collection.Find(filter).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<PreOrderModel>> GetAllProductsAsync(CancellationToken cancellationToken = default)
    {
        var collection = GetCollection<PreOrderModel>(CollectionName);
        return await collection.Find(_ => true).ToListAsync(cancellationToken);
    }

    public async Task<bool> UpdateProductAsync(PreOrderModel preOrder, CancellationToken cancellationToken = default)
    {
        var collection = GetCollection<PreOrderModel>(CollectionName);
        var filter = Builders<PreOrderModel>.Filter.Eq(p => p.Code, preOrder.Code);
        var result = await collection.ReplaceOneAsync(filter, preOrder, cancellationToken: cancellationToken);
        return result.IsAcknowledged && result.ModifiedCount > 0;
    }

    public async Task<bool> DeleteProductAsync(string id, CancellationToken cancellationToken = default)
    {
        var collection = GetCollection<PreOrderModel>(CollectionName);
        var filter = Builders<PreOrderModel>.Filter.Eq(p => p.Code, id);
        var result = await collection.DeleteOneAsync(filter, cancellationToken);
        return result.IsAcknowledged && result.DeletedCount > 0;
    }
}
