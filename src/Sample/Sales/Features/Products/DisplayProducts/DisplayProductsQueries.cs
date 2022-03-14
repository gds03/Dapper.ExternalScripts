using System.Data;
using System.Data.SqlClient;

using Dapper;
using Dapper.ExternalScripts;
using Dapper.ExternalScripts.Attributes;

namespace Sales.Features.Products.DisplayProducts;

[DapperSearchRoute("Features/Products/DisplayProducts/SQL")]
public class DisplayProductsQueries
{
    private readonly IExternalFileFinder<DisplayProductsQueries> scriptFinder;
    private readonly SalesConnectionString salesConnectionString;

    public DisplayProductsQueries(IExternalFileFinder<DisplayProductsQueries> scriptFinder, SalesConnectionString salesConnectionString)
    {
        this.scriptFinder = scriptFinder;
        this.salesConnectionString = salesConnectionString;
    }

    public async Task<IEnumerable<DisplayProductVM>> GetAll()
    {
        using IDbConnection connection = new SqlConnection(salesConnectionString.Value);

        return await connection.QueryAsync<DisplayProductVM>(scriptFinder.GetCurrentScript());
    }

    [DapperRename("GetOne")]
    public async Task<DisplayProductVM> GetSingle(int id)
    {
        using IDbConnection connection = new SqlConnection(salesConnectionString.Value);

        return await connection.QuerySingleAsync<DisplayProductVM>(scriptFinder.GetCurrentScript(), new { id = id });
    }
}
