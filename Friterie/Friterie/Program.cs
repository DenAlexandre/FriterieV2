using Blazorise;
using Blazorise.Bootstrap5;
using Blazorise.Icons.FontAwesome;
using Friterie.Authentication;
using Friterie.BlazorServer.Services;
using Friterie.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.Identity.Web;
using MudBlazor.Services;
using Serilog;
using System.Net.Http.Headers;



var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Ajouter l'authentification avec Microsoft Entra ID
builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"));

builder.Services.AddAuthorizationCore();

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();


// Services applicatifs
builder.Services.AddScoped<ApiService>();
builder.Services.AddScoped<CartService>();
builder.Services.AddScoped<AuthStateService>();
builder.Services.AddScoped<ProtectedSessionStorage>();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
//builder.Services.AddSingleton<UserServiceView>();
//builder.Services.AddSingleton<UserAccountService>();
builder.Services.AddSingleton<WeatherForecastService>();







builder.Services.AddMudServices();

// Enregistrer IHttpContextAccessor pour l'accès au contexte HTTP
builder.Services.AddHttpContextAccessor();
builder.Services
    .AddBlazorise(options =>
    {
        options.Immediate = true;
    })
    .AddBootstrap5Providers()
    .AddFontAwesomeIcons();

// Register Fluent UI services
builder.Services.AddFluentUIComponents();





// Session pour maintenir l'état
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddHttpClient("FriterieAPI", client =>
{
    client.BaseAddress = new Uri("https://localhost:5001/FriterieAPI");
    client.DefaultRequestHeaders.Accept.Clear();
    client.DefaultRequestHeaders.Accept.Add(
        new MediaTypeWithQualityHeaderValue("application/json"));
});

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

// Middleware d'authentification et d'autorisation
app.UseAuthentication();
app.UseAuthorization();

////Pour remettre l'authentification Azure, il faut faire la redirection dans le MainLayout.razor
//// Endpoint pour se connecter
//app.MapGet("/authentication/login", async (HttpContext context) =>
//{
//    await context.ChallengeAsync(OpenIdConnectDefaults.AuthenticationScheme, new AuthenticationProperties
//    {
//        RedirectUri = "/login"
//    });
//});

//// Endpoint pour se déconnecter
//app.MapGet("/authentication/logout", async (HttpContext context) =>
//{
//    await context.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme, new AuthenticationProperties
//    {
//        RedirectUri = "/"
//    });
//});

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");
app.Run();
