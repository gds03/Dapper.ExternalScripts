using Dapper.ExternalScripts.Configuration;

using System.Runtime.CompilerServices;

namespace Dapper.ExternalScripts;

public class ScriptFinder<TSource> : IScriptFinder<TSource>
{
    private readonly Dictionary<string, string> _map = new Dictionary<string, string>();    // maps method names to file contents

    public ScriptFinder(ScriptFinderGlobalConfiguration globalConfiguration)
    {
        ScriptFinderTypeConfiguration<TSource>? typeConfiguration = null;
        if (globalConfiguration == null || !globalConfiguration.TryGetConfigurationFor<TSource>(out typeConfiguration))
        {
            throw new InvalidOperationException($"There is no configuration for type '{typeof(TSource)}' present in '{typeof(ScriptFinderGlobalConfiguration)}'");
        }

        _map = MapFromConfiguration(typeConfiguration!);
    }


    public string GetCurrentScript([CallerMemberName] string? methodName = null)
    {
        if (!_map.TryGetValue(methodName!, out var value))
            throw new InvalidOperationException($"can't find method name {methodName} in the cache");

        return value;
}

    public TResult _<TResult>(Func2<TResult> callback, [CallerMemberName] string? methodName = null)
    {
        string script = this.GetCurrentScript(methodName);
        return callback(script);
    }

    #region Helpers

    private Dictionary<string, string> MapFromConfiguration(ScriptFinderTypeConfiguration<TSource> typeConfiguration)
    {
        string directoryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, typeConfiguration.Route!.Replace("/", "\\"));
        if (!Directory.Exists(directoryPath))
            throw new DirectoryNotFoundException($"Directory {directoryPath} does not exists");


        var map = new Dictionary<string, string>();


        var exceptionsList = new List<Exception>();

        foreach(var methodKVP in typeConfiguration.MethodMaps)
        {
            var entry = methodKVP.Value;

            var fileName = entry.RenamedName;
            var fileExtension = entry.Extension;

            Exception? fileException = GetFileContentsForMethod(GetFilePath(directoryPath, fileName!, fileExtension!), out var fileContents);
            if (fileException != null)
                exceptionsList.Add(fileException);

            else
                if (!TryAdd(map, methodKVP.Key, fileContents!, out Exception? exception))
                exceptionsList.Add(exception!);

        }

        if (exceptionsList.Any())
            throw new AggregateException(exceptionsList);

        return map;

        #region helpers



        static string GetFilePath(string directoryPath, string fileName, string fileExtension) =>
            Path.ChangeExtension(Path.Combine(directoryPath, fileName), fileExtension);

        static Exception? GetFileContentsForMethod(string finalFilePath, out string? fileContents)
        {
            fileContents = null;
            try
            {
                fileContents = File.ReadAllText(finalFilePath);
                return null;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }
        static bool TryAdd(Dictionary<string, string> mapping, string key, string fileContents, out Exception? exception)
        {
            exception = null;
            try
            {
                mapping.Add(key, fileContents);
                return true;
            }

            catch (Exception ex)
            {
                exception = ex;
                return false;
            }
        }

        #endregion
    }

    #endregion
}
