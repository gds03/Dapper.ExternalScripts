using System.Reflection;

namespace Dapper.ExternalScripts.Configuration;
public class ScriptFinderTypeConfiguration<TSource>
{
    public string? Route { get; private set; }
    public string? ScriptsExtension { get; private set; }
    public Dictionary<string, string> MethodMaps { get; private set; } = new Dictionary<string, string>();



    public ScriptFinderTypeConfiguration<TSource> SetRoute(string route)
    {
        if (string.IsNullOrWhiteSpace(route))
           throw new ArgumentException($"{nameof(route)} can't be empty");
        
        route = route.Trim();
        var charArray = route.ToCharArray();

        if (charArray.Any(c => char.IsWhiteSpace(c)))
            throw new ArgumentException($"{nameof(route)} can't contain spaces.");

        if (charArray.Any(c => c == '\\'))
            throw new ArgumentException($"{nameof(route)} can't contain any backward slash. Use forward slash");

        if (charArray.First() == '/')
            throw new ArgumentException($"{nameof(route)} first char must start with the directory name and not the '/' character");

        if (charArray.Count(c => c == '/') == 0)
            throw new ArgumentException($"{nameof(route)} must have at least one '/' to specify the first level folder");

        Route = route;
        return this;
    }

    public ScriptFinderTypeConfiguration<TSource> SetScriptsExtension(string extension)
    {
        if (string.IsNullOrWhiteSpace(extension))
            throw new ArgumentException($"{nameof(extension)} can't be empty");

        var scriptsExtensionChars = extension.Trim().ToCharArray();
        if (scriptsExtensionChars.Any(c => char.IsWhiteSpace(c)))
            throw new ArgumentException($"{nameof(extension)} can't contain spaces.");

        if (scriptsExtensionChars.Any(c => c == '/' || c == '\\' || c == '.'))
            throw new ArgumentException($"{nameof(extension)} can't contain any '\\' or '/' or '.' characters");

        ScriptsExtension = extension;
        return this;
    }

    public ScriptFinderTypeConfiguration<TSource> AutoMap()
    {
        foreach (var method in typeof(TSource).GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
        {
            var methodName = method.Name;
            MethodMaps[methodName] = methodName;
        }

        return this;
    }

    public ScriptFinderTypeConfiguration<TSource> Rename(string from, string to)
    {
        if (string.IsNullOrWhiteSpace(from))
            throw new ArgumentException($"{nameof(from)} can't be empty.");

        if (string.IsNullOrWhiteSpace(to))
            throw new ArgumentException($"{nameof(to)} can't be empty.");

        if(!MethodMaps.ContainsKey(from))
            throw new InvalidOperationException($"{nameof(from)} is not mapped. Are you forgetting to call {nameof(AutoMap)}?");

        var fileNameChars = to.Trim().ToCharArray();

        if (fileNameChars.Any(c => char.IsWhiteSpace(c)))
            throw new ArgumentException($"{nameof(to)} can't contain spaces.");

        if (fileNameChars.Any(c => c == '/' || c == '\\'))
            throw new ArgumentException($"{nameof(to)} can't contain any '\' or '/' characters");

        MethodMaps[from] = to;
        return this;
    }
}
