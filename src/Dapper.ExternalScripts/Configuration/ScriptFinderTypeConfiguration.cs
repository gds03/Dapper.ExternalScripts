using System.Data;
using System.Reflection;
using System.Text;

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

        Route = route;
        return this;
    }

    public ScriptFinderTypeConfiguration<TSource> SetScriptsExtension(string extension)
    {
        if (string.IsNullOrWhiteSpace(extension))
            throw new ArgumentException($"{nameof(extension)} can't be empty");

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

        if (!MethodMaps.ContainsKey(from))
            throw new InvalidOperationException($"{nameof(from)} is not mapped. Are you forgetting to call {nameof(AutoMap)}?");

        MethodMaps[from] = to;
        return this;
    }

    public bool IsValid(out string errorMsg)
    {
        StringBuilder errors = new StringBuilder();

        if (!IsRouteValid(out var routeErrors))
            errors.AppendLine(routeErrors);

        if (!IsScriptsExtensionValid(out var scriptsExtensionErrors))
            errors.AppendLine(scriptsExtensionErrors);

        if (!AreRenamesValid(out var renamesErrors))
            errors.AppendLine(renamesErrors);

        errorMsg = errors.ToString();
        return string.IsNullOrEmpty(errorMsg);
    }

    #region Helpers

    private bool IsRouteValid(out string errors)
    {
        StringBuilder s = new();

        if (string.IsNullOrWhiteSpace(Route))
            s.AppendLine($"{nameof(Route)} can't be empty");

        else
        {
            var charArray = Route!.ToCharArray();

            if (charArray.Any(c => char.IsWhiteSpace(c)))
                s.AppendLine($"{nameof(Route)} can't contain spaces.");

            if (charArray.Any(c => c == '\\'))
                s.AppendLine($"{nameof(Route)} can't contain any backward slash. Use forward slash");

            if (charArray.First() == '/')
                s.AppendLine($"{nameof(Route)} first char must start with the directory name and not the '/' character");

            if (charArray.Count(c => c == '/') == 0)
                s.AppendLine($"{nameof(Route)} must have at least one '/' to specify the first level folder");
        }

        errors = s.ToString();
        return string.IsNullOrEmpty(errors);
    }

    private bool IsScriptsExtensionValid(out string errors)
    {
        StringBuilder s = new();

        if (string.IsNullOrWhiteSpace(ScriptsExtension))
            s.AppendLine($"{nameof(ScriptsExtension)} can't be empty");

        else
        {
            var scriptsExtensionChars = ScriptsExtension!.Trim().ToCharArray();
            if (scriptsExtensionChars.Any(c => char.IsWhiteSpace(c)))
                s.AppendLine($"{nameof(ScriptsExtension)} can't contain spaces.");

            if (scriptsExtensionChars.Any(c => c == '/' || c == '\\' || c == '.'))
                s.AppendLine($"{nameof(ScriptsExtension)} can't contain any '\\' or '/' or '.' characters");
        }

        errors = s.ToString();
        return string.IsNullOrEmpty(errors);
    }

    public bool AreRenamesValid(out string errors)
    {
        StringBuilder s = new();

        foreach (var to in MethodMaps.Values)
        {
            var fileNameChars = to.Trim().ToCharArray();

            if (fileNameChars.Any(c => char.IsWhiteSpace(c)))
                s.AppendLine($"{nameof(to)} can't contain spaces.");

            if (fileNameChars.Any(c => c == '/' || c == '\\'))
                s.AppendLine($"{nameof(to)} can't contain any '\' or '/' characters");
        }

        errors = s.ToString();
        return string.IsNullOrEmpty(errors);
    }

    #endregion
}
