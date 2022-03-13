using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

using Newtonsoft.Json;

using Sales.Features.Products.AppendProduct;
using Sales.Features.Products.DisplayProducts;

namespace Sales;

public static class Endpoints
{
    public static void MapSales(this IEndpointRouteBuilder endpoints)
    {
        // READ
        endpoints.MapGet("/sales/products/displayproducts", async context =>
        {
            var displayQueries = context.RequestServices.GetRequiredService<DisplayProductsQueries>();


            var result = await displayQueries.GetAll();

            await context.Response.WriteAsync(JsonConvert.SerializeObject(result));
        });

        endpoints.MapGet("/sales/products/display_one_product", async context =>
        {
            var displayQueries = context.RequestServices.GetRequiredService<DisplayProductsQueries>();


            var result = await displayQueries.GetSingle(1);

            await context.Response.WriteAsync(JsonConvert.SerializeObject(result));
        });


        // WRITE
        endpoints.MapPost("/sales/products/insert_one_product", async context =>
        {
            var appendProductCommands = context.RequestServices.GetRequiredService<AppendProductCommands>();


            var productName = context.Request.Form["ProductName"];
            var quantity = int.Parse(context.Request.Form["Quantity"]);


            var result = await appendProductCommands.Insert(productName, quantity);

            await context.Response.WriteAsync(JsonConvert.SerializeObject(result));
        });
    }
}

