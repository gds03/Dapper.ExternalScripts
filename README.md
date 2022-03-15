# Dapper.ExternalScripts
Instead of hardcore sql in your CS files which is hard to maintain, leave them in a outside .sql file and grab them as you need.


Example usage: Please check the Sample project API.

Wiring up on Program.cs or Startup.cs

```csharp

 serviceCollection.AddDapperExternalScripts(options =>
            options
                .Configure<DisplayProductsQueries>(x =>
                {
                    x
                        .SetRoute("Features/Products/DisplayProducts/SQL")
                        .Rename(nameof(DisplayProductsQueries.GetSingle), "GetOne")
                        .SetExtension(nameof(DisplayProductsQueries.GetAll), "sql");
                })
                .Configure<AppendProductCommands>(x =>
                {
                    x
                        .SetRoute("Features/Products/AppendProduct/SQL")
                        .Rename(nameof(AppendProductCommands.Insert), "InsertOneProduct");
                })

        );
   
```

On your repositories, commands or queries (mostly)
```csharp


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

        return await scriptFinder._((sql) => connection.QueryAsync<DisplayProductVM>(sql)); // instead of hardcoded SQL it already loaded GetAll.sql from the folder above
    }

    public async Task<DisplayProductVM> GetSingle(int id)
    {
        using IDbConnection connection = new SqlConnection(salesConnectionString.Value);

        return await scriptFinder._(sql => connection.QuerySingleAsync<DisplayProductVM>(sql, new { id = id })); // instead of hardcoded SQL it already loaded GetOne.sql from the folder above
    }
}

   
```

The way its working is similiar with ILogger<T> regarding registration.
   
It's registered as a singleton and the first time the type is referenced (in this case: IScriptFinder<DisplayProductsQueries>) it will load from the FS.
From there on, the script is in the cache and thats all.
   
For mapping the types use the FluentAPI when registering the services. 
   - The required argument is Route
   - The default extension is sql
   - You can rename methods to files with the use of Rename method
   - One caveat is that overload is not supported.
