# Dapper.ExternalScripts
Instead of hardcore sql in your CS files which is hard to maintain, leave them in a outside .sql file and grab them as you need.


Example usage: Please check the Sample project API.

Wiring up on Program.cs or Startup.cs

```csharp

builder.Services.AddDapperExternalScripts();
   
```

On your repositories, commands or queries (mostly)
```csharp

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

        return await connection.QueryAsync<DisplayProductVM>(scriptFinder.GetCurrentScript()); // instead of hardcoded SQL it already loaded GetAll.sql from the folder above
    }

    [DapperRename("GetOne")]
    public async Task<DisplayProductVM> GetSingle(int id)
    {
        using IDbConnection connection = new SqlConnection(salesConnectionString.Value);

        return await connection.QuerySingleAsync<DisplayProductVM>(scriptFinder.GetCurrentScript(), new { id = id }); // instead of hardcoded SQL it already loaded GetOne.sql from the folder above
    }
}

   
```

The way its working is similiar with ILogger<T>. It's registered as a singleton and the first time the type is referenced (in this case: IExternalFileFinder<DisplayProductsQueries>) it will load from the FS.
From there on, the script is in the cache and thats all.
