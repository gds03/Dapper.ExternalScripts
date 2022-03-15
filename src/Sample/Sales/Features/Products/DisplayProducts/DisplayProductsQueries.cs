using System.Data;
using System.Data.SqlClient;

using Dapper;
using Dapper.ExternalScripts;

namespace Sales.Features.Products.DisplayProducts;

public class DisplayProductsQueries
{
    private readonly IScriptFinder<DisplayProductsQueries> scriptFinder;
    private readonly SalesConnectionString salesConnectionString;

    public DisplayProductsQueries(IScriptFinder<DisplayProductsQueries> scriptFinder, SalesConnectionString salesConnectionString)
    {
        this.scriptFinder = scriptFinder;
        this.salesConnectionString = salesConnectionString;
    }

    public async Task<IEnumerable<DisplayProductVM>> GetAll()
    {
        using IDbConnection connection = new SqlConnection(salesConnectionString.Value);

        return await scriptFinder._((sql) => connection.QueryAsync<DisplayProductVM>(sql));
    }

    public async Task<DisplayProductVM> GetSingle(int id)
    {
        using IDbConnection connection = new SqlConnection(salesConnectionString.Value);

        return await connection.QuerySingleAsync<DisplayProductVM>(scriptFinder.GetCurrentScript(), new { id = id });
    }
}
