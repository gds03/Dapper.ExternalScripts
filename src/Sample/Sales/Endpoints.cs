using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

using Newtonsoft.Json;

using Sales.Features.Products.DisplayProducts;

namespace Sales;

public static class Endpoints
{
    public static void MapSales(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/sales/products/displayproducts", async context =>
        {
            var displayQueries = context.RequestServices.GetRequiredService<DisplayProductsQueries>();


            var result = await displayQueries.GetAll();

            await context.Response.WriteAsync(JsonConvert.SerializeObject(result));
        });
    }
}
