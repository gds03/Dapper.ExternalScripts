using System.Runtime.CompilerServices;

namespace Dapper.ExternalScripts;

/// <summary>
///     Gets a script for a type TSource at the method level.
/// </summary>
/// <typeparam name="TSource"></typeparam>
public interface IExternalFileFinder<TSource>
{
    string GetScript([CallerMemberName] string? methodName = null);
}
