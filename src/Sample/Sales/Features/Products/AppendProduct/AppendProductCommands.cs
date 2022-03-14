using System.Data.SqlClient;
using System.Data;
using Dapper.ExternalScripts.Attributes;
using Dapper.ExternalScripts;
using Dapper;

namespace Sales.Features.Products.AppendProduct;

[DapperSearchRoute("Features/Products/AppendProduct/SQL")]
public class AppendProductCommands
{
    private readonly IExternalFileFinder<AppendProductCommands> scriptFinder;
    private readonly SalesConnectionString salesConnectionString;

    public AppendProductCommands(IExternalFileFinder<AppendProductCommands> scriptFinder, SalesConnectionString salesConnectionString)
    {
        this.scriptFinder = scriptFinder;
        this.salesConnectionString = salesConnectionString;
    }

    [DapperRename("InsertOneProduct")]
    public async Task<int> Insert(string name, int quantity)
    {
        using IDbConnection connection = new SqlConnection(salesConnectionString.Value);

        return await connection.ExecuteAsync(scriptFinder.GetCurrentScript(), new { Name = name, quantity = quantity });
    }
}
