global using BlazingPizza.Shared;
global using BlazingPizza;
using BlazingPizza.Client;
using BlazingPizza.Components;
using BlazingPizza.Components.Account;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using BlazingPizza.Shared.Interfaces;
using BlazingPizza.Repositories;
using BlazingPizza.Services;
using BlazingPizza.Middleware;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
        .AddInteractiveServerComponents()
        .AddInteractiveWebAssemblyComponents();


// Add Security
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<AuthenticationStateProvider, BlazingPizza.Components.Account.PersistingRevalidatingAuthenticationStateProvider>();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.ExpireTimeSpan = TimeSpan.FromDays(7);
        options.SlidingExpiration = true;
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
        options.Cookie.Name = ".AspNetCore.Cookies";
        options.Cookie.Path = "/";
        options.Events.OnRedirectToLogin = context =>
        {
            context.Response.Redirect("/Account/Login");
            return Task.CompletedTask;
        };
        options.Events.OnRedirectToLogout = context =>
        {
            context.Response.Redirect("/Account/Logout");
            return Task.CompletedTask;
        };
    });

builder.Services.AddDbContext<PizzaStoreContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ICarrierRepository , CarrierRepository>();
builder.Services.AddScoped<INotificationRepository , NotificationRepository>();
builder.Services.AddScoped<IRefillCardRepository , RefillCardRepository>();
builder.Services.AddScoped<IRechargeRepository , RechargeRepository>();
builder.Services.AddScoped<ITransactionRepository , TransactionRepository>();
builder.Services.AddScoped<IInternetPackageRepository, BlazingPizza.Repositories.InternetPackageRepository>();
builder.Services.AddScoped<IInternetPackagePurchaseRepository, BlazingPizza.Repositories.InternetPackagePurchaseRepository>();

// Add image service
builder.Services.AddScoped<IImageService, ImageService>();

// Add notification service
builder.Services.AddScoped<BlazingPizza.Services.NotificationService>();

// Add PIN services
builder.Services.AddScoped<PinService>();
builder.Services.AddScoped<PinSessionService>(serviceProvider =>
{
    var httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();
    // ProtectedSessionStorage is only available in Blazor Server components context
    var protectedSessionStorage = serviceProvider.GetService<ProtectedSessionStorage>();
    return new PinSessionService(httpContextAccessor, protectedSessionStorage);
});

builder.Services.AddScoped<HttpClient>(sp =>
{
    var navigationManager = sp.GetRequiredService<Microsoft.AspNetCore.Components.NavigationManager>();
    return new HttpClient { BaseAddress = new Uri(navigationManager.BaseUri) };
});

builder.Services.AddControllers();

builder.Services.AddHttpContextAccessor();

// Add distributed memory cache for session support
builder.Services.AddDistributedMemoryCache();

// Add session support for PIN verification
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.Name = ".AspNetCore.Session";
    options.Cookie.SameSite = SameSiteMode.Lax;
});

builder.Services.AddAntiforgery();

var app = builder.Build();

// Initialize the database
var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
using (var scope = scopeFactory.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<PizzaStoreContext>();
    db.Database.EnsureCreated();
    SeedData.Initialize(db);
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseSession();

app.UseAntiforgery();

app.UseAuthentication();
app.UseAuthorization();

// Add PIN protection middleware - Disabled: Now handled by individual page components
// app.UseMiddleware<PinProtectionMiddleware>();

app.MapPizzaApi();

app.MapControllers();

app.MapRazorComponents<App>()
        .AddInteractiveServerRenderMode()
        .AddInteractiveWebAssemblyRenderMode()
        .AddAdditionalAssemblies(typeof(BlazingPizza.Client._Imports).Assembly);

app.Run();