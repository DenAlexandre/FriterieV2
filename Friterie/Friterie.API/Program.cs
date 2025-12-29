using Friterie.API.Services;
using Friterie.API.Stores;
using Friterie.BlazorServer.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using System;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuration JWT
var jwtKey = builder.Configuration["Jwt:Key"];
var key = Encoding.UTF8.GetBytes(jwtKey);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };
    });





builder.Services.AddAuthorization();

// Enregistrement des services en Singleton (données en mémoire)
builder.Services.AddSingleton<DataService>();

builder.Services.AddScoped<IUserStore, UserStore>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<AuthStateService>();

builder.Services.AddScoped<IProductStore, ProductStore>();
builder.Services.AddScoped<ProductService>();

builder.Services.AddScoped<IOrderStore, OrderStore>();
builder.Services.AddScoped<OrderService>();

builder.Services.AddScoped<CartService>();
builder.Services.AddScoped<PaymentService>();


// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowedHosts", policy =>
    {
        policy.WithOrigins("https://localhost:5001")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});



builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition(
                    "Bearer",
                    new OpenApiSecurityScheme
                    {
                        In = ParameterLocation.Header,
                        Description = "Please enter a valid token.",
                        Name = "Authorization",
                        Type = SecuritySchemeType.Http,
                        BearerFormat = "JWT",
                        Scheme = "Bearer",
                    }
                );

    options.AddSecurityRequirement(document => new() { [new OpenApiSecuritySchemeReference("Bearer", document)] = [] });


});

//Auth cookie
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/login";          // page de login
        options.AccessDeniedPath = "/forbidden"; // page forbidden
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.SlidingExpiration = true;
    });

builder.Services.AddAuthorization();


var app = builder.Build();

// Initialiser les données
var dataService = app.Services.GetRequiredService<DataService>();
dataService.InitializeData();

if (app.Environment.IsDevelopment())
{
    //if (env.IsDevelopment())
    //{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
       // c.SwaggerEndpoint("/swagger/v1/swagger.json", "Catalog V1");
    });

    RewriteOptions option = new();
    option.AddRedirect("^$", "swagger");
    app.UseRewriter(option);
    //}

    //app.UseHttpsRedirection();
    app.UseRouting();

    app.UseAuthentication(); // Ensure Authentication is before Authorization
                             // app.UseMiddleware<JwtMiddleware>(); // JWT Middleware should be placed here
                             // app.UseAuthorization();
    app.UseAuthorization();

    //app.UseEndpoints(endpoints =>
    //{
    //    endpoints.MapControllers();
    //});

}

app.UseHttpsRedirection();
app.UseCors("AllowedHosts");
app.MapControllers();

app.Run();