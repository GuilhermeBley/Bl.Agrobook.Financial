using Bl.Agrobook.Financial.Func.Model;
using Bl.Agrobook.Financial.Func.Model.Kyte;
using Bl.Agrobook.Financial.Func.Options;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Bl.Agrobook.Financial.Func.Services;
public class FinancialKyteApiService
{
    private const string DefaultBaseUrlDefaultBaseUrl = "https://kyte-api-gateway.azure-api.net";
    private const string DefaultOrign = "https://web.kyteapp.com";

    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        WriteIndented = false
    };

    private readonly HttpClient _httpClient;
    private readonly IOptions<KyteOptions> _options;


    public FinancialKyteApiService(
        IHttpClientFactory httpClientFactory,
        IOptions<KyteOptions> options)
    {
        _httpClient = httpClientFactory.CreateClient("FinancialKyteApiService");
        _httpClient.BaseAddress = new Uri(DefaultBaseUrlDefaultBaseUrl);
        _httpClient.DefaultRequestHeaders.Add("ocp-apim-trace", "true");
        _httpClient.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/139.0.0.0 Safari/537.36");
        _httpClient.DefaultRequestHeaders.Add("connection", "keep-alive");
        _httpClient.DefaultRequestHeaders.Add("origin", DefaultOrign);
        _httpClient.DefaultRequestHeaders.Add("accept", "application/json, text/plain, */*");
        _options = options;
    }

    public async IAsyncEnumerable<GetProductModel> GetProductsAsync([EnumeratorCancellation]CancellationToken cancellationToken = default)
    {
        var auth = KyteAuthenticationInfo.Create(_options.Value);
        var skip = 0;
        const int limit = 80;
        var hasMore = true;

        do
        {
            var request = new HttpRequestMessage(
                HttpMethod.Get,
                $"api/kyte-web/products/{auth.LocalId}?limit={limit}&stockStatus=&categoryId=&sort=PIN_FIRST&skip={skip}&search=&isWeb=1");
            AddAuthorizationHeaders(request, auth);
            using var response = await _httpClient.SendAsync(request, cancellationToken);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<GetProductRootModel>(_jsonOptions, cancellationToken);

            if (result?.Products == null) yield break;

            foreach (var content in result.Products)
                yield return content;

            hasMore = result.Products.Count == limit;
        } while (hasMore);
    }

    public async IAsyncEnumerable<GetCustomerCustomerModel> GetCustomersAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var auth = KyteAuthenticationInfo.Create(_options.Value);
        var skip = 0;
        const int limit = 80;
        var hasMore = true;

        do
        {
            var request = new HttpRequestMessage(
                HttpMethod.Get,
                $"api/kyte-web/products/{auth.LocalId}?limit={limit}&stockStatus=&categoryId=&sort=PIN_FIRST&skip={skip}&search=&isWeb=1");
            AddAuthorizationHeaders(request, auth);
            using var response = await _httpClient.SendAsync(request, cancellationToken);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<GetCustomerModel>(_jsonOptions, cancellationToken);

            if (result?.Customers == null) yield break;

            foreach (var content in result.Customers)
                yield return content;

            hasMore = result.Customers.Count == limit;
        } while (hasMore);
    }

    public async Task<JsonNode> CreateOrderAsync(
        GetCustomerCustomerModel customer,
        CreateCartProductModel[] products)
    {

    }

    private void AddAuthorizationHeaders(HttpRequestMessage request, KyteAuthenticationInfo auth)
    {
        request.Headers.Add("uid", auth.Uid);
        request.Headers.Add("ocp-apim-subscription-key", auth.Key);
    }

    private class KyteAuthenticationInfo
    {
        private readonly KyteOptions _options;
        public string Uid => _options.Uid;
        public string LocalId => string.Concat(_options.Uid.Take(14));
        public string Key => _options.SubscriptionKey;

        private KyteAuthenticationInfo(KyteOptions options)
        {
            _options = options;
        }

        public static KyteAuthenticationInfo Create(KyteOptions options)
        {
            if (string.IsNullOrEmpty(options.Uid) || options.Uid.Length < 14)
                throw new ArgumentNullException(nameof(options.Uid), "Uid is required");
            if (string.IsNullOrEmpty(options.SubscriptionKey))
                throw new ArgumentNullException(nameof(options.SubscriptionKey), "Key is required");
            return new KyteAuthenticationInfo(options);
        }
    }
}
