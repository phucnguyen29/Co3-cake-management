using WebAPI.Services.Interfaces;
using WebClient.Service;
using WebClient.Service.Interfaces;
using WebClient.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebClient.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<WebClientContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("WebClientContext") ?? throw new InvalidOperationException("Connection string 'WebClientContext' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient<IProductService, ProductService>();
builder.Services.AddHttpClient<IAuthService, AuthService>();

builder.Services.AddHttpContextAccessor();
// Required for in-memory session storage
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Products/Index");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// Enable session after routing and before endpoints
app.UseSession();

app.UseRouting();

app.UseAuthorization();

app.UseHttpsRedirection();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Products}/{action=Index}/{id?}");

app.MapGet("/ping", () => "OK");
app.Run();
