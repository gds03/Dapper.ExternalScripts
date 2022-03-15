namespace Dapper.ExternalScripts.Attributes;
/// <summary>
///     Marks and specifies the location for the service to look for the files.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class ScriptRouteAttribute : Attribute
{
    public string Route { get; }

    public ScriptRouteAttribute(string route)
    {
        if (string.IsNullOrEmpty(route))
            throw new ArgumentException(nameof(route));

        route = route.Trim();
        var charArray = route.ToCharArray();

        if (charArray.Any(c => char.IsWhiteSpace(c)))
            throw new InvalidOperationException($"{nameof(route)} can't contain spaces.");

        if (charArray.Any(c => c == '\\'))
            throw new InvalidOperationException($"{nameof(route)} can't contain any backward slash. Use forward slash");

        if (charArray.First() == '/')
            throw new InvalidOperationException($"{nameof(route)} first char must start with the directory name and not the '/' character");

        if (charArray.Count(c => c == '/') == 0)
            throw new InvalidOperationException($"{nameof(route)} must have at least one '/' to specify the first level folder");

        this.Route = route;
    }
}
