namespace Friterie.BlazorServer.Services;

using Friterie.Shared.Models;

//using Friterie.API.Models;
using System.Net.Http.Headers;
using System.Net.Http.Json;
//using Order = Shared.Models.Order;
//using OrderItem = Shared.Models.OrderItem;
//using Product = Shared.Models.Product;

public class ApiService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly AuthStateService _authStateService;

    public ApiService(IHttpClientFactory httpClientFactory, AuthStateService authStateService)
    {
        _httpClientFactory = httpClientFactory;
        _authStateService = authStateService;
    }

    private HttpClient CreateClient()
    {
        var client = _httpClientFactory.CreateClient("FriterieAPI");
        var token = _authStateService.GetToken();

        if (!string.IsNullOrEmpty(token))
        {
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }

        return client;
    }

    // ============= AUTH =============

    public async Task<LoginResponse?> LoginAsync(string email, string password)
    {
        var client = CreateClient();
        var response = await client.PostAsJsonAsync("/api/auth/login", new { email, password });

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<LoginResponse>();
        }
        return null;
    }

    public async Task<bool> RegisterAsync(RegisterRequest request)
    {
        var client = CreateClient();
        var response = await client.PostAsJsonAsync("/api/auth/register", request);
        return response.IsSuccessStatusCode;
    }

    // ============= PRODUCTS =============

    public async Task<List<Product>> GetProductsAsync()
    {
        var client = CreateClient();
        var products = await client.GetFromJsonAsync<List<Product>>("/api/products");
        return products ?? new List<Product>();
    }

    public async Task<List<Product>> GetProductsByCategoryAsync(string category)
    {
        var client = CreateClient();
        var products = await client.GetFromJsonAsync<List<Product>>($"/api/products/category/{category}");
        return products ?? new List<Product>();
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {
        var client = CreateClient();
        return await client.GetFromJsonAsync<Product>($"/api/products/{id}");
    }

    // ============= ORDERS =============

    public async Task<Order?> CreateOrderAsync(List<OrderItem> items)
    {
        var client = CreateClient();
        var response = await client.PostAsJsonAsync("/api/orders", new { items });

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<Order>();
        }
        return null;
    }

    public async Task<Order?> GetOrderAsync(int orderId)
    {
        var client = CreateClient();
        return await client.GetFromJsonAsync<Order>($"/api/orders/{orderId}");
    }

    public async Task<List<Order>> GetUserOrdersAsync()
    {
        var client = CreateClient();
        var orders = await client.GetFromJsonAsync<List<Order>>("/api/orders/user");
        return orders ?? new List<Order>();
    }

    // ============= PAYMENT =============

    public async Task<PaymentIntentResponse?> CreatePaymentIntentAsync(decimal amount)
    {
        var client = CreateClient();
        var response = await client.PostAsJsonAsync("/api/payment/create-intent", new { amount });

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<PaymentIntentResponse>();
        }
        return null;
    }

    public async Task<bool> ConfirmPaymentAsync(int orderId, string paymentIntentId)
    {
        var client = CreateClient();
        var response = await client.PostAsJsonAsync("/api/payment/confirm", new { orderId, paymentIntentId });
        return response.IsSuccessStatusCode;
    }
}

// ============= DTOs =============

public class LoginResponse
{
    public string Token { get; set; } = string.Empty;
    public UserInfoServer User { get; set; } = new();
}

public class UserInfoServer
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
}

public class RegisterRequest
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
}

public class PaymentIntentResponse
{
    public string ClientSecret { get; set; } = string.Empty;
    public string PaymentIntentId { get; set; } = string.Empty;
}