using System.Data.SqlClient;
using System.Data;
using Dapper.ExternalScripts.Attributes;
using Dapper.ExternalScripts;
using Dapper;

namespace Sales.Features.Products.AppendProduct;

[ScriptRoute("Features/Products/AppendProduct/SQL")]
public class AppendProductCommands
{
    private readonly IScriptFinder<AppendProductCommands> scriptFinder;
    private readonly SalesConnectionString salesConnectionString;

    public AppendProductCommands(IScriptFinder<AppendProductCommands> scriptFinder, SalesConnectionString salesConnectionString)
    {
        this.scriptFinder = scriptFinder;
        this.salesConnectionString = salesConnectionString;
    }

    [ScriptRename("InsertOneProduct")]
    public async Task<int> Insert(string name, int quantity)
    {
        using IDbConnection connection = new SqlConnection(salesConnectionString.Value);

        return await connection.ExecuteAsync(scriptFinder.GetCurrentScript(), new { Name = name, quantity = quantity });
    }
}
