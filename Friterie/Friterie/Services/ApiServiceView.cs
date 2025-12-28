namespace Friterie.BlazorServer.Services;


using Friterie.Shared.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http.Json;

public class ApiServiceView
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly AuthStateServiceView _authStateService;


    private const string GET_LOGIN_BDD = "FriterieAPI/api/auth/login";
    private const string REGISTER_BDD = "FriterieAPI/api/auth/register";




    private const string REMOVE_PRODUCT_IN_ORDER = "FriterieAPI/api/orders/remove-product";
    private const string ADD_PRODUCT_IN_ORDER = "FriterieAPI/api/orders/add-product";
    private const string ADD_ITEMS_IN_ORDER = "FriterieAPI/api/add-items-in-order";
    private const string ADD_ORDER = "FriterieAPI/api/add-order";
    private const string GET_ORDER_BY_USER_ID = "FriterieAPI/api/get-order-by-user-id";

    private const string GET_PRODUCTS_BDD = "/FriterieAPI/api/products/GetProducts";
    private const string GET_PRODUCTS_BY_CATEGORY_BDD = "/FriterieAPI/api/products/category";
    private const string GET_PRODUCTS_BY_ID_BDD = "/FriterieAPI/api/products/";

    public ApiServiceView(IHttpClientFactory httpClientFactory, AuthStateServiceView authStateService)
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
    #region Auth
    public async Task<LoginResponse?> LoginAsync(string email, string password)
    {
        var client = CreateClient();

        var response = await client.PostAsJsonAsync(GET_LOGIN_BDD, new { email, password });

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<LoginResponse>();
        }
        return null;
    }

    public async Task<bool> RegisterAsync(RegisterRequest request)
    {
        var client = CreateClient();
        var response = await client.PostAsJsonAsync(REGISTER_BDD, request);
        return response.IsSuccessStatusCode;
    }
    #endregion


    #region Products

    // ============= PRODUCTS =============

    public async Task<List<Product>> GetProductsAsync(int type, int limit, int offset)
    {
        var client = CreateClient();

        var requestUri = $"{GET_PRODUCTS_BDD}" + $"?type={type}&limit={limit}&offset={offset}";
        var jsonResponse = await client.GetStringAsync(requestUri);

        var products = JsonConvert.DeserializeObject<List<Product>>(jsonResponse);


        //var products = await client.GetFromJsonAsync<List<Product>>(GET_PRODUCTS_BDD + "/"  + new { type, limit, offset });
        return products ?? new List<Product>();
    }

    public async Task<List<Product>> GetProductsByCategoryAsync(string category)
    {
        var client = CreateClient();
        var products = await client.GetFromJsonAsync<List<Product>>(GET_PRODUCTS_BY_CATEGORY_BDD + "/" + category);
        return products ?? new List<Product>();
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {
        var client = CreateClient();
        return await client.GetFromJsonAsync<Product>(GET_PRODUCTS_BY_ID_BDD + "/" + id);
    }

    #endregion


    #region orders
    // ============= ORDERS =============


    public async Task RemoveProductInOrderAsync(int orderId, int productId)
    {
        var client = CreateClient();
        var response = await client.PostAsJsonAsync(REMOVE_PRODUCT_IN_ORDER, new { orderId, productId });

        if (response.IsSuccessStatusCode)
        {
            //return await response.Content.ReadFromJsonAsync<Orders>();
        }
        //return null;
    }

    public async Task AddProductInOrderIdAsync(int orderId, int  productId, int quantity)
    {
        var client = CreateClient();
        var response = await client.PostAsJsonAsync(ADD_PRODUCT_IN_ORDER, new { orderId, productId, quantity });

        if (response.IsSuccessStatusCode)
        {
            //return await response.Content.ReadFromJsonAsync<Orders>();
        }
        //return null;
    }



    public async Task<Order?> CreateItemsInOrderAsync(List<OrderItem> items)
    {
        var client = CreateClient();
        var response = await client.PostAsJsonAsync(ADD_ITEMS_IN_ORDER, new { items });

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<Order>();
        }
        return null;
    }


    public async Task<int?> CreateOrderAsync(int userId)
    {
        var client = CreateClient();
        var response = await client.PostAsJsonAsync(ADD_ORDER, userId);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<int>();
        }
        return null;
    }

    //public async Task<Order?> GetOrderAsync(int orderId)
    //{
    //    var client = CreateClient();
    //    return await client.GetFromJsonAsync<Order>($GET_ORDER_BY_USER_ID +  "/{orderId}");
    //}

    public async Task<List<Order>> GetOrdersByUserIdAsync(int userId)
    {
        var client = CreateClient();
        var orders = await client.GetFromJsonAsync<List<Order>>(GET_ORDER_BY_USER_ID + $"/{userId}");
        return orders ?? new List<Order>();
    }
    #endregion


    #region payment
    // ============= PAYMENT =============

    public async Task<PaymentIntentResponse?> CreatePaymentIntentAsync(decimal amount)
    {
        var client = CreateClient();
        var response = await client.PostAsJsonAsync("FriterieAPI/api/payment/create-intent", new { amount });

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<PaymentIntentResponse>();
        }
        return null;
    }

    public async Task<bool> ConfirmPaymentAsync(int orderId, string paymentIntentId)
    {
        var client = CreateClient();
        var response = await client.PostAsJsonAsync("FriterieAPIapi/payment/confirm", new { orderId, paymentIntentId });
        return response.IsSuccessStatusCode;
    }

    #endregion
}

// ============= DTOs =============

public class LoginResponse
{
    public string Token { get; set; } = string.Empty;
    public User User { get; set; } = new();
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