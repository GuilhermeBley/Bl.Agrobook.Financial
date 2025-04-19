using Bl.Agrobook.Financial.Func.Options;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace Bl.Agrobook.Financial.Func.Services;

internal class FinancialApiService
{
    private readonly HttpClient _client;
    private readonly FinancialApiOptions _options;
    private readonly ILogger<FinancialApiService> _logger;

    public FinancialApiService(
        IOptions<FinancialApiOptions> options,
        IHttpClientFactory factory,
        ILogger<FinancialApiService> logger)
    {
        _client = factory.CreateClient("FinancialApi");
        _client.BaseAddress = new(options.Value.BaseUrl);
        _options = options.Value;
        _logger = logger;
    }

    public async IAsyncEnumerable<JsonElement> GetProductsAsync(
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        await EnsureApiAuthenticatedAsync(cancellationToken);


    }

    private async Task EnsureApiAuthenticatedAsync(CancellationToken cancellationToken = default)
    {

    }
}
