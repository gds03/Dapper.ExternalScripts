using Dapper.Conventions;

using Sales;
using Sales.Features.Products.AppendProduct;
using Sales.Features.Products.DisplayProducts;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddSales();
builder.Services.AddDapperExternalScripts(options =>
    options
        .Configure<DisplayProductsQueries>(x =>
        {
            x
                .SetRoute("Features/Products/DisplayProducts/SQL")
               // .SetScriptsExtension("sql")
                .AutoMap()
                .Rename("GetSingle", "GetOne");
        })
        .Configure<AppendProductCommands>(x =>
        {
            x
                .SetRoute("Features/Products/AppendProduct/SQL")
                .SetScriptsExtension("sql")
                .AutoMap()
                .Rename("Insert", "InsertOneProduct");
        })

);

// Add services to the container.

var app = builder.Build();

app.MapSales();

app.Run();
