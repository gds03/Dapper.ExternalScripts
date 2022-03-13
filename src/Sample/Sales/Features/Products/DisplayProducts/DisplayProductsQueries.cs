using System.Data;
using System.Data.SqlClient;

using Dapper;

namespace Sales.Features.Products.DisplayProducts;
public class DisplayProductsQueries
{
    private readonly SalesConnectionString salesConnectionString;

    public DisplayProductsQueries(SalesConnectionString salesConnectionString)
    {
        this.salesConnectionString = salesConnectionString;
    }

    public async Task<IEnumerable<DisplayProductVM>> GetAll()
    {
        using (IDbConnection connection = new SqlConnection(salesConnectionString.Value))
        {
            return await connection.QueryAsync<DisplayProductVM>("select [id], [name], [Quantity] as qty from product"); // convert this later.
        }
    }
}
