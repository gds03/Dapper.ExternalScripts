﻿using Dapper.ExternalScripts.Attributes;

using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Dapper.ExternalScripts;

public class ScriptFinder<TSource> : IScriptFinder<TSource>
{
    private readonly Dictionary<string, string> _map;

    public ScriptFinder()
    {
        var dapperPathAttribute = typeof(TSource).GetCustomAttribute<ScriptRouteAttribute>();
        if (dapperPathAttribute == null)
            throw new InvalidOperationException($"To use this service please Mark {typeof(TSource).Name} class with {typeof(ScriptRouteAttribute).Name} attribute and define a location for the scripts");

        string directoryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, dapperPathAttribute.Route.Replace("/", "\\"));
        if (!Directory.Exists(directoryPath))
            throw new DirectoryNotFoundException($"Directory {directoryPath} does not exists");


        var map = new Dictionary<string, string>();


        var exceptionsList = new List<Exception>();


        foreach (var method in typeof(TSource).GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
        {
            var fileName = method.Name;
            var extension = "sql";
            var dapperRenameAttribute = method.GetCustomAttribute<ScriptRenameAttribute>();
            if(dapperRenameAttribute != null)
            {
                fileName = dapperRenameAttribute.FileName;
                extension = dapperRenameAttribute.FileExtension;
            }

            Exception? fileException = GetFileContentsForMethod(GetFilePath(directoryPath, fileName, extension), out var fileContents);
            if (fileException != null)
                exceptionsList.Add(fileException);

            else
                if (!TryAdd(map, method.Name, fileContents!, out Exception? exception))
                    exceptionsList.Add(exception!);
        }

        if (exceptionsList.Any())
            throw new AggregateException(exceptionsList);

        _map = map;
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
    private static string GetFilePath(string directoryPath, string fileName, string fileExtension) => Path.ChangeExtension(Path.Combine(directoryPath, fileName), fileExtension);

    private static Exception? GetFileContentsForMethod(string finalFilePath, out string? fileContents)
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
    private static bool TryAdd(Dictionary<string, string> mapping, string key, string fileContents, out Exception? exception)
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