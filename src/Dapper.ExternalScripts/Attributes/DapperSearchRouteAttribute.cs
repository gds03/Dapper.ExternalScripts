namespace Dapper.ExternalScripts.Attributes;
/// <summary>
///     Marks and specifies the location for the service to look for the files.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class DapperSearchRouteAttribute : Attribute
{
    public string Route { get; }

    public DapperSearchRouteAttribute(string route)
    {
        if (string.IsNullOrEmpty(route))
            throw new ArgumentException(nameof(route));

        if (route.Trim().ToCharArray().Any(c => char.IsWhiteSpace(c)))
            throw new InvalidOperationException($"route can't contain spaces.");

        this.Route = route;
    }
}
