using System.Reflection;
using System.Text;

namespace Dapper.ExternalScripts.Configuration;

public class ScriptFinderTypeConfiguration<TSource>
{
    public string? Route { get; private set; }
    public Dictionary<string, TypeMethodConfiguration> MethodMaps { get; private set; } = new Dictionary<string, TypeMethodConfiguration>();



    public ScriptFinderTypeConfiguration()
    {
        MethodMaps = this.GetMethodMappings();
    }



    public ScriptFinderTypeConfiguration<TSource> SetRoute(string route)
    {
        if (string.IsNullOrWhiteSpace(route))
            throw new ArgumentException($"{nameof(route)} can't be empty");

        Route = route;
        return this;
    }


    public ScriptFinderTypeConfiguration<TSource> SetExtension(string fromMethodName, string extension)
    {
        if (string.IsNullOrWhiteSpace(fromMethodName))
            throw new ArgumentException($"{nameof(fromMethodName)} can't be empty.");

        if (string.IsNullOrWhiteSpace(extension))
            throw new ArgumentException($"{nameof(extension)} can't be empty.");

        if (!MethodMaps.TryGetValue(fromMethodName, out var entry))
            throw new InvalidOperationException($"'{fromMethodName}' method do not exists");


        entry.Extension = extension;
        return this;
    }

    public ScriptFinderTypeConfiguration<TSource> Rename(string fromMethodName, string toFileName, string? extension = null)
    {
        if (string.IsNullOrWhiteSpace(fromMethodName))
            throw new ArgumentException($"{nameof(fromMethodName)} can't be empty.");

        if (string.IsNullOrWhiteSpace(toFileName))
            throw new ArgumentException($"{nameof(toFileName)} can't be empty.");

        if (!MethodMaps.TryGetValue(fromMethodName, out var entry))
            throw new InvalidOperationException($"'{fromMethodName}' method do not exists");

        entry.RenamedName = toFileName;

        if(!string.IsNullOrWhiteSpace(extension))
            entry.Extension = extension;

        return this;
    }

    public bool IsValid(out string errorMsg)
    {
        StringBuilder errors = new StringBuilder();

        if (!IsRouteValid(out var routeErrors))
            errors.AppendLine(routeErrors);

        if (!AreMappingsValid(out var mappingErrors))
            errors.AppendLine(mappingErrors);

        errorMsg = errors.ToString();
        return string.IsNullOrEmpty(errorMsg);
    }

    #region Helpers

    private Dictionary<string, TypeMethodConfiguration> GetMethodMappings()
    {
        var map = new Dictionary<string, TypeMethodConfiguration>();
        foreach (var method in typeof(TSource).GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
        {
            var methodName = method.Name;
            map[methodName] = new TypeMethodConfiguration() { RenamedName = methodName };
        }

        return map;
    }

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

    public bool AreMappingsValid(out string errors)
    {
        StringBuilder s = new();

        foreach (var entry in MethodMaps.Values)
        {
            var renamed = entry.RenamedName!;
            var fileNameChars = renamed.Trim().ToCharArray();

            if (fileNameChars.Any(c => char.IsWhiteSpace(c)))
                s.AppendLine($"{nameof(renamed)} can't contain spaces.");

            if (fileNameChars.Any(c => c == '/' || c == '\\'))
                s.AppendLine($"{nameof(renamed)} can't contain any '\' or '/' characters");


            var extensionChars = entry.Extension!.Trim().ToCharArray();
            if (extensionChars.Any(c => char.IsWhiteSpace(c)))
                s.AppendLine($"{nameof(entry.Extension)} can't contain spaces.");

            if (extensionChars.Any(c => c == '/' || c == '\\' || c == '.'))
                s.AppendLine($"{nameof(entry.Extension)} can't contain any '\\' or '/' or '.' characters");
        }

        errors = s.ToString();
        return string.IsNullOrEmpty(errors);
    }

    #endregion
}
