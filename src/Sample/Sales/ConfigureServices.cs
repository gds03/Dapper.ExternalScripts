using Dapper.ExternalScripts;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Sales.Features.Products.AppendProduct;
using Sales.Features.Products.DisplayProducts;

namespace Sales;
public static class ConfigureServices
{
    public static void AddSales(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddTransient<DisplayProductsQueries>();
        serviceCollection.AddTransient<AppendProductCommands>();

        serviceCollection.AddSingleton<SalesConnectionString>(di => new SalesConnectionString
        {
            Value = di.GetRequiredService<IConfiguration>().GetConnectionString("default")
        });

        serviceCollection.AddDapperExternalScripts(options =>
            options
                .Configure<DisplayProductsQueries>(x =>
                {
                    x
                        .SetRoute("Features/Products/DisplayProducts/SQL")
                        .SetScriptsExtension("sql")
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
    }
}