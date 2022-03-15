using System.Runtime.CompilerServices;

namespace Dapper.ExternalScripts;

/// <summary>
///     Gets a script for a type TSource at the method level.
/// </summary>
/// <typeparam name="TSource"></typeparam>
public interface IScriptFinder<TSource>
{
    string GetCurrentScript([CallerMemberName] string? methodName = null);
    TResult _<TResult>(Func2<TResult> callback, [CallerMemberName] string? methodName = null);
}

public delegate TResult Func2<TResult>(string script);