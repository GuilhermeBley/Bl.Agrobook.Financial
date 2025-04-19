using Bl.Agrobook.Financial.Func.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;
using System.Text.Json.Nodes;

namespace Bl.Agrobook.Financial.Func.Services;

internal class FinancialApiService
{
    private static readonly SemaphoreSlim _semaphore = new(1, 1);
    private static JsonSerializerOptions _jsonSerializerOptions = new()
    {
    };

    private readonly HttpClient _client;
    private readonly FinancialApiOptions _options;
    private readonly ILogger<FinancialApiService> _logger;

    public FinancialApiService(
        IOptions<FinancialApiOptions> options,
        IHttpClientFactory factory,
        ILogger<FinancialApiService> logger)
    {
        _client = factory.CreateClient("FinancialApi");
        _client.BaseAddress = new(options.Value.BaseUrl );
        _client.DefaultRequestHeaders.Add("Accept", "application/json");
        _client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/135.0.0.0 Safari/537.36");
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
        if (!IsTokenExpired(_client.DefaultRequestHeaders.Authorization?.Parameter))
        {
            return;
        }

        try
        {
            await _semaphore.WaitAsync(cancellationToken);

            if (!IsTokenExpired(_client.DefaultRequestHeaders.Authorization?.Parameter))
            {
                return;
            }

            using var request = new HttpRequestMessage(HttpMethod.Post, "api/v1/authenticate")
            {
                Content = JsonContent.Create(new
                {
                    login = _options.Login,
                    password = _options.Password
                },
                options: _jsonSerializerOptions)
            };

            request.Headers.Add("client", "NEX-AUTH");

            var response = await _client.SendAsync(request, cancellationToken);

            if (!response.IsSuccessStatusCode || response.Content.Headers.ContentType?.ToString() != "application/json")
            {
                var body = await response.Content.ReadAsStringAsync(cancellationToken);
                throw new HttpRequestException(
                    $"Failed to authenticate with the financial API. See the error body:\n{body}");
            }

            var content = await response.Content.ReadFromJsonAsync<JsonNode>(cancellationToken);

            var token = content?["token"]?.ToString();

            if (IsTokenExpired(token))
            {
                throw new HttpRequestException($"Failed to authenticate with the financial API. Token '{token}' is invalid.");
            }

            _client.DefaultRequestHeaders.Authorization = new("Bearer", token);
        }
        finally
        {
            _semaphore.Release();
        }
    }

    private static bool IsTokenExpired(string? jwtToken)
    {
        var expirationTime = GetTokenExpirationTime(jwtToken);
        return expirationTime.HasValue && expirationTime.Value.AddMinutes(-2) < DateTime.UtcNow;
    }

    private static DateTime? GetTokenExpirationTime(string? jwtToken)
    {
        if (string.IsNullOrEmpty(jwtToken))
        {
            return null;
        }

        var handler = new JwtSecurityTokenHandler();

        if (handler.CanReadToken(jwtToken))
        {
            var token = handler.ReadJwtToken(jwtToken);

            // Get the expiration time (convert from Unix timestamp)
            if (token.Payload.Expiration.HasValue)
            {
                var expirationTime = DateTimeOffset.FromUnixTimeSeconds(token.Payload.Expiration.Value).DateTime;
                return expirationTime;
            }
        }

        return null;
    }
}
