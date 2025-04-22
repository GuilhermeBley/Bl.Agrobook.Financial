using Azure.Data.Tables;
using Bl.Agrobook.Financial.Func.Model;
using Bl.Agrobook.Financial.Func.Options;
using Microsoft.Extensions.Options;

namespace Bl.Agrobook.Financial.Func.Services;

public class AgrobookAuthRepository 
{
    public readonly TableClient _tableClient;

    public AgrobookAuthRepository(IOptions<AzureTableOption> config)
    {
        _tableClient = new(config.Value.StorageKey, "AgrobookAuthRepositoryasndaoind");
        _tableClient.CreateIfNotExists();
    }

    public async Task<AzureTableAuthModel?> GetLastTokenAsync(CancellationToken cancellationToken = default)
    {
        await foreach (var entity in _tableClient.QueryAsync<AzureTableAuthModel>(maxPerPage: 1))
            return entity;

        return null;
    }

    public async Task UpdateTokenAsync(string token, CancellationToken cancellationToken = default)
    {
        await _tableClient.UpsertEntityAsync(new AzureTableAuthModel() { Token = token },mode: TableUpdateMode.Replace, cancellationToken: cancellationToken);
    }
}
