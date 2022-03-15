
using Sales;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSales();

// Add services to the container.

var app = builder.Build();

app.MapSales();

app.Run();
