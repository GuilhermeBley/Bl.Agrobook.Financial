using Bl.Agrobook.Financial.Func.Model;
using Bl.Agrobook.Financial.Func.Options;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Bl.Agrobook.Financial.Func.Repositories;

public class DeliveryDateRepository : RepositoryBase
{
    private const string CollectionName = "DeliveryDate";

    public DeliveryDateRepository(IOptions<MongoDbOptions> options) : base(options)
    {
    }

    public async Task InsertAsync(DeliveryDateModel model, CancellationToken cancellationToken = default)
    {
        var collection = GetCollection<DeliveryDateModel>(CollectionName);
        await collection.InsertOneAsync(model, new() { }, cancellationToken);
    }

    public async Task InsertManyAsync(IEnumerable<DeliveryDateModel> model, CancellationToken cancellationToken = default)
    {
        var collection = GetCollection<DeliveryDateModel>(CollectionName);
        await collection.InsertManyAsync(model, new() { }, cancellationToken);
    }

    public async Task<DeliveryDateModel> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var collection = GetCollection<DeliveryDateModel>(CollectionName);
        var filter = Builders<DeliveryDateModel>.Filter.Eq(p => p.Id, id);
        return await collection.Find(filter).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<DeliveryDateModel>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var collection = GetCollection<DeliveryDateModel>(CollectionName);
        return await collection.Find(_ => true).ToListAsync(cancellationToken);
    }

    public async Task<bool> UpdateAsync(DeliveryDateModel model, CancellationToken cancellationToken = default)
    {
        var collection = GetCollection<DeliveryDateModel>(CollectionName);
        var filter = Builders<DeliveryDateModel>.Filter.Eq(p => p.Id, model.Id);
        var result = await collection.ReplaceOneAsync(filter, model, cancellationToken: cancellationToken);
        return result.IsAcknowledged && result.ModifiedCount > 0;
    }

    public async Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        var collection = GetCollection<DeliveryDateModel>(CollectionName);
        var filter = Builders<DeliveryDateModel>.Filter.Eq(p => p.Id, id);
        var result = await collection.DeleteOneAsync(filter, cancellationToken);
        return result.IsAcknowledged && result.DeletedCount > 0;
    }

    public async Task<IEnumerable<DeliveryDateModel>> GetAllGreatherThanAsync(DateOnly date, CancellationToken cancellationToken = default)
    {
        var collection = GetCollection<DeliveryDateModel>(CollectionName);
        var filter = Builders<DeliveryDateModel>.Filter.Eq(p => p.DeliveryAt, date);

        return await collection.Find(filter).ToListAsync(cancellationToken);
    }
}
