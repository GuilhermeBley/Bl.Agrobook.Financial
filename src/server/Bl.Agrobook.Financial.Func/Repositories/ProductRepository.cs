using Bl.Agrobook.Financial.Func.Model;
using Bl.Agrobook.Financial.Func.Options;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Bl.Agrobook.Financial.Func.Repositories;

public class ProductRepository : RepositoryBase
{
    private const string CollectionName = "Products";

    public ProductRepository(IOptions<MongoDbOptions> options) : base(options)
    {
    }

    public async Task InsertProductAsync(ProductModel product, CancellationToken cancellationToken = default)
    {
        var collection = GetCollection<ProductModel>(CollectionName);
        await collection.InsertOneAsync(product, new() { }, cancellationToken);
    }

    public async Task InsertManyProductsAsync(IEnumerable<ProductModel> products, CancellationToken cancellationToken = default)
    {
        var collection = GetCollection<ProductModel>(CollectionName);
        await collection.InsertManyAsync(products, new() { }, cancellationToken);
    }

    public async Task<ProductModel> GetProductByIdAsync(string productCode, CancellationToken cancellationToken = default)
    {
        var collection = GetCollection<ProductModel>(CollectionName);
        var filter = Builders<ProductModel>.Filter.Eq(p => p.Code, productCode);
        return await collection.Find(filter).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<ProductModel>> GetAllProductsAsync(CancellationToken cancellationToken = default)
    {
        var collection = GetCollection<ProductModel>(CollectionName);
        return await collection.Find(_ => true).ToListAsync(cancellationToken);
    }

    public async Task<bool> UpdateProductAsync(ProductModel product, CancellationToken cancellationToken = default)
    {
        var collection = GetCollection<ProductModel>(CollectionName);
        var filter = Builders<ProductModel>.Filter.Eq(p => p.Code, product.Code);
        var result = await collection.ReplaceOneAsync(filter, product, cancellationToken: cancellationToken);
        return result.IsAcknowledged && result.ModifiedCount > 0;
    }

    public async Task<bool> DeleteProductAsync(string id, CancellationToken cancellationToken = default)
    {
        var collection = GetCollection<ProductModel>(CollectionName);
        var filter = Builders<ProductModel>.Filter.Eq(p => p.Code, id);
        var result = await collection.DeleteOneAsync(filter, cancellationToken);
        return result.IsAcknowledged && result.DeletedCount > 0;
    }
}
