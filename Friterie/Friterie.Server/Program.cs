using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Friterie.Server.Stores;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<IFriterieStore, FriterieStore>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();


// Active Swagger en dev
if (app.Environment.IsDevelopment())
{

    app.UseExceptionHandler("/Error");
    //https://localhost:7031/swagger/index.html
}
app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();
