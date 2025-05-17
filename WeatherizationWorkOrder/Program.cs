using WeatherizationWorkOrder.Business;
using WeatherizationWorkOrder.Data;
using WeatherizationWorkOrder.Data.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();

builder.Services.AddSingleton<IInventoryProvider, InventoryProvider>();
builder.Services.AddSingleton<IInventoryDataProvider, InventoryDataProvider>();
builder.Services.AddSingleton<IUserDataProvider, UserDataProvider>();
builder.Services.AddSingleton<IWorkOrderDataProvider, WorkOrderDataProvider>();
builder.Services.AddSingleton<IWorkOrderProvider, WorkOrderProvider>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.Run();
