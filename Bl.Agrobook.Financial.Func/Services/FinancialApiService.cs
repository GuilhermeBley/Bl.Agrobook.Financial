using Bl.Agrobook.Financial.Func.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;
using System.Text.Json.Nodes;
using Bl.Agrobook.Financial.Func.Model;
using System.Text.Json.Serialization;

namespace Bl.Agrobook.Financial.Func.Services;

public class FinancialApiService
{
    private static readonly SemaphoreSlim _semaphore = new(1, 1);
    private static JsonSerializerOptions _jsonSerializerOptions = new()
    {
        WriteIndented = true,
        Converters = { new IsoDateTimeConverter() },
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
        _client.BaseAddress = new(options.Value.BaseUrl);
        _client.DefaultRequestHeaders.Add("Accept", "application/json");
        _client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/135.0.0.0 Safari/537.36");
        _options = options.Value;
        _logger = logger;
    }

    public async IAsyncEnumerable<ProductViewModel> GetProductsAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var userInfo = await EnsureApiAuthenticatedAsync(cancellationToken);

        var page = 0;
        const int pageSize = 50;
        do
        {
            using var response = await _client.GetAsync($"api/v1/product/{userInfo.ShopCode}?page={page}&size={pageSize}&sort=description&direction=asc&pdv=false&filter=active", cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException(message:
                    $"Invalid status code {response.StatusCode} and body: {await response.Content.ReadAsStringAsync(cancellationToken)}");
            }

            var result = await response.Content.ReadFromJsonAsync<GetProductsViewModel>(_jsonSerializerOptions, cancellationToken)
                ?? throw new HttpRequestException("Invalid status code 200 body response.");

            foreach (var c in result.Content)
            {
                yield return c;
            }

            if (result.TotalPages == (page + 1) || result.Content.Count == 0)
            {
                break;
            }

            page++;

        } while (!cancellationToken.IsCancellationRequested);
    }

    public async IAsyncEnumerable<CustomerViewModel> GetCustomersAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var userInfo = await EnsureApiAuthenticatedAsync(cancellationToken);

        var page = 0;
        const int pageSize = 50;
        do
        {
            using var response = await _client.GetAsync($"api/v1/customer/{userInfo.ShopCode}?&page={page}&size={pageSize}&sort=name&direction=asc&&filter=active&stats=true", cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException(message:
                    $"Invalid status code {response.StatusCode} and body: {await response.Content.ReadAsStringAsync(cancellationToken)}");
            }

            var result = await response.Content.ReadFromJsonAsync<GetCustomerViewModel>(_jsonSerializerOptions, cancellationToken)
                ?? throw new HttpRequestException("Invalid status code 200 body response.");

            foreach (var c in result.Content)
            {
                yield return c;
            }

            if (result.TotalPages == (page + 1) || result.Content.Count == 0)
            {
                break;
            }

            page++;

        } while (!cancellationToken.IsCancellationRequested);
    }

    public async Task<JsonNode> CreateOrderAsync(
        CreateCustomerOrderViewModel order,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var userInfo = await EnsureApiAuthenticatedAsync(cancellationToken);

        using var request = new HttpRequestMessage(HttpMethod.Post, $"api/v1/order/{userInfo.ShopCode}")
        {
            // add the body            
            Content = JsonContent.Create(
                order,
                options: _jsonSerializerOptions)
        };

        var response = await _client.SendAsync(request, cancellationToken);

        if (!response.IsSuccessStatusCode || response.Content.Headers.ContentType?.ToString() != "application/json")
        {
            var body = await response.Content.ReadAsStringAsync(cancellationToken);
            throw new HttpRequestException(
                $"Failed to create order with the financial API. See the error body:\n{body}");
        }

        return await response.Content.ReadFromJsonAsync<JsonNode>(cancellationToken)
            ?? throw new HttpRequestException("Invalid status code 200 body response.");
    }

    public async IAsyncEnumerable<SaleHistoryViewModel> GetOrdersAsync(
        bool alreadyOpen = true,
        [EnumeratorCancellation]CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var userInfo = await EnsureApiAuthenticatedAsync(cancellationToken);

        var page = 0;
        const int pageSize = 50;
        do
        {
            var status = new List<int>();
            DateTime? datebegin = null;
            DateTime? dateend = null;

            if (alreadyOpen) status.Add(1);

            using var response = await _client.PostAsJsonAsync(
                $"api/v1/order/{userInfo.ShopCode}/historic?page={page}&size={pageSize}&sort=included_on&direction=desc",
                new
                {
                    canceled = false,
                    datebegin = datebegin,
                    dateend = dateend,
                    origin = Array.Empty<object>(),
                    search = "",
                    status = status
                },
                cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException(message:
                    $"Invalid status code {response.StatusCode} and body: {await response.Content.ReadAsStringAsync(cancellationToken)}");
            }

            var result = await response.Content.ReadFromJsonAsync<GetSalesHistoryViewModel>(_jsonSerializerOptions, cancellationToken)
                ?? throw new HttpRequestException("Invalid status code 200 body response.");

            foreach (var c in result.Orders.Content)
            {
                yield return c;
            }

            if (result.Orders.TotalPages == (page + 1) || result.Orders.Content.Count == 0)
            {
                break;
            }

            page++;

        } while (!cancellationToken.IsCancellationRequested);
    }

    private async Task<InternalUserInfo> EnsureApiAuthenticatedAsync(CancellationToken cancellationToken = default)
    {
        var token = _client.DefaultRequestHeaders.Authorization?.Parameter;
        if (!IsTokenExpired(token))
        {
            return InternalUserInfo.CreateByToken(token);
        }

        try
        {
            await _semaphore.WaitAsync(cancellationToken);

            if (!IsTokenExpired(_client.DefaultRequestHeaders.Authorization?.Parameter))
            {
                return InternalUserInfo.CreateByToken(token);
            }

            var authurl = string.Concat(_options.AuthBaseUrl.Trim('/'), '/', "api/v1/authenticate");

            using var request = new HttpRequestMessage(HttpMethod.Post, new Uri(authurl))
            {
                Content = JsonContent.Create(new
                {
                    origin = 3,
                    username = _options.Login,
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

            token = content?["token"]?.ToString();

            if (IsTokenExpired(token))
            {
                throw new HttpRequestException($"Failed to authenticate with the financial API. Token '{token}' is invalid.");
            }

            _client.DefaultRequestHeaders.Authorization = new("Bearer", token);

            return InternalUserInfo.CreateByToken(token);
        }
        finally
        {
            _semaphore.Release();
        }
    }

    private static bool IsTokenExpired(string? jwtToken)
    {
        if (string.IsNullOrEmpty(jwtToken)) return true;

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

    private record InternalUserInfo(
        int ShopCode,
        string Email)
    {

        public static InternalUserInfo CreateByToken(string? jwtToken)
        {
            ArgumentException.ThrowIfNullOrEmpty(jwtToken);
            var handler = new JwtSecurityTokenHandler();

            if (!handler.CanReadToken(jwtToken))
                throw new InvalidOperationException($"Invalid token: {jwtToken}");

            var token = handler.ReadJwtToken(jwtToken);

            return new(int.Parse(token.Payload["shopcode"].ToString() ?? string.Empty), token.Payload.Sub);
        }
    }

    private class IsoDateTimeConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return DateTime.Parse(reader.GetString() ?? string.Empty);
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss.fffZ"));
        }
    }
}
