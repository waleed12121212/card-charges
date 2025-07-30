global using BlazingPizza.Shared;
global using BlazingPizza.Client;

using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using BlazingPizza.Shared.Interfaces;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// Configure HttpClient to use the base address of the server project
builder.Services.AddScoped<HttpClient>(sp =>
    new HttpClient
    {
        BaseAddress = new Uri("https://localhost:64019/")
    });

builder.Services.AddScoped<IRefillCardRepository , HttpRefillCardRepository>();
builder.Services.AddScoped<ICarrierRepository , HttpCarrierRepository>();
builder.Services.AddScoped<INotificationRepository , HttpNotificationRepository>();
builder.Services.AddScoped<ITransactionRepository , HttpTransactionRepository>();
builder.Services.AddScoped<IRechargeRepository , HttpRechargeRepository>();
builder.Services.AddScoped<IInternetPackageRepository, BlazingPizza.Client.Repositories.HttpInternetPackageRepository>();
builder.Services.AddScoped<IInternetPackagePurchaseRepository, BlazingPizza.Client.Repositories.HttpInternetPackagePurchaseRepository>();

// Add Security
builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddSingleton<AuthenticationStateProvider , PersistentAuthenticationStateProvider>();


await builder.Build().RunAsync();
