using Dapper.Conventions;

using Sales;




var builder = WebApplication.CreateBuilder(args);



builder.Services.AddSales();
builder.Services.AddDapperExternalScripts();

// Add services to the container.

var app = builder.Build();

app.MapSales();

app.Run();
