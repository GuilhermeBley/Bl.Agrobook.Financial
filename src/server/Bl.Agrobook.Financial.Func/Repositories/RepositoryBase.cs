using Bl.Agrobook.Financial.Func.Options;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Security.Authentication;
using System.Xml.Xsl;

namespace Bl.Agrobook.Financial.Func.Repositories;

internal class RepositoryBase
{
    private const string DatabaseName = "Agrobook";

    private MongoClient _mongoClient;

    public RepositoryBase(IOptions<MongoDbOptions> options)
    {
        var settings = MongoClientSettings.FromUrl(new MongoUrl(options.Value.ConnectionString));
        settings.SslSettings = new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };
        var mongoClient = new MongoClient(settings);
        _mongoClient = mongoClient;
    }

    protected IMongoDatabase GetDatabase()
    {
        var db = _mongoClient.GetDatabase(DatabaseName);
        return db;
    }

    protected IMongoCollection<T> GetCollection<T>(string collectionName)
    {
        var db = GetDatabase();

        var collection = db.GetCollection<T>(collectionName);

        return collection;
    }
}
