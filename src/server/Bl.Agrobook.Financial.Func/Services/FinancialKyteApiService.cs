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
            skip += limit;
        } while (hasMore);
    }

    public async IAsyncEnumerable<GetCustomerCustomerModel> GetCustomersAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var auth = KyteAuthenticationInfo.Create(_options.Value);
        var skip = 0;
        const int limit = 40;
        var hasMore = true;

        do
        {
            var request = new HttpRequestMessage(
                HttpMethod.Get,
                $"api/kyte-web/customer/{auth.LocalId}/list?sort=A_Z&filter=&limit={limit}&skip={skip}");
            AddAuthorizationHeaders(request, auth);
            using var response = await _httpClient.SendAsync(request, cancellationToken);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<GetCustomerModel>(_jsonOptions, cancellationToken);

            if (result?.Customers == null) yield break;

            foreach (var content in result.Customers)
                yield return content;

            hasMore = result.Customers.Count == limit;
            skip += limit;
        } while (hasMore);
    }

    /// <summary>
    /// Create a new order. It's important keep in mind that Kyte doesn't support concurrent orders creation. 
    /// So you'll be able to just create one order by time for each account.
    /// </summary>
    public async Task<JsonNode> CreateOrderAsync(
        GetCustomerCustomerModel customer,
        CreateCartProductModel[] products,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(customer, nameof(customer));
        ArgumentNullException.ThrowIfNull(products, nameof(products));
        ArgumentOutOfRangeException.ThrowIfZero(products.Length, nameof(products));

        // 
        // In kyte web, you can just create an order by managing your cart.
        // So first, you'll need to fill your cart and then finish.
        // 
        var auth = KyteAuthenticationInfo.Create(_options.Value);

        // First, clear the cart to ensure no one conflict
        using var deleteReq = new HttpRequestMessage(HttpMethod.Delete, "api/kyte-web/cart");
        AddAuthorizationHeaders(deleteReq, auth);
        using var deleteResp = await _httpClient.SendAsync(deleteReq, cancellationToken);
        deleteResp.EnsureSuccessStatusCode();

        // Second, Add a customer
        using var customerReq = new HttpRequestMessage(HttpMethod.Post, $"api/kyte-web/cart/customer/{customer.Id}");
        AddAuthorizationHeaders(customerReq, auth);
        using var customerResp = await _httpClient.SendAsync(customerReq, cancellationToken);
        customerResp.EnsureSuccessStatusCode();

        int prodCount = 1;
        // Then, add all the products
        foreach (var product in products)
        {
            try
            {
                using var productReq = new HttpRequestMessage(HttpMethod.Post, $"api/kyte-web/cart/product")
                {
                    Content = JsonContent.Create(product, options: _jsonOptions)
                };
                AddAuthorizationHeaders(productReq, auth);
                using var prodResp = await _httpClient.SendAsync(productReq, cancellationToken);
                prodResp.EnsureSuccessStatusCode();
                await Task.Delay(50); // small delay to avoid crawler detection
            }
            catch
            {
                throw new HttpRequestException($"Falha em adicionar produto {prodCount} em pedido.");
            }
            prodCount++;
        }

        using var cartReq = new HttpRequestMessage(HttpMethod.Post, $"api/kyte-web/cart");
        AddAuthorizationHeaders(cartReq, auth);
        using var cartResp = await _httpClient.SendAsync(cartReq, cancellationToken);
        cartResp.EnsureSuccessStatusCode();
        var cartDetails = await cartResp.Content.ReadFromJsonAsync<JsonNode>()
            ?? throw new HttpRequestException("Falha em coleta de carrinho.");

        // Finishing cart to complete the order, completing as order and not as sell
        var finishingOrderObj = new FinishSaleRequestModel()
        {
            Payments = [
                FinishSalePaymentRequestModel.CreateOther(products.Sum(p => p.TotalPrice), 0)
            ],
            PayBack = 0,
            TotalPay = products.Sum(p => p.TotalPrice),
        };
        using var finishingCartReq = new HttpRequestMessage(HttpMethod.Post, $"api/kyte-web/cart/finish-sale")
        {
            Content = JsonContent.Create(finishingOrderObj, options: _jsonOptions)
        };
        AddAuthorizationHeaders(finishingCartReq, auth);
        using var finishingCartResp = await _httpClient.SendAsync(finishingCartReq, cancellationToken);
        finishingCartResp.EnsureSuccessStatusCode();

        return cartDetails;
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
