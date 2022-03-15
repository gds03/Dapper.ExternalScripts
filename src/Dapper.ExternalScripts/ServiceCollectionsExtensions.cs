using Dapper.ExternalScripts.Configuration;

using Microsoft.Extensions.DependencyInjection;

namespace Dapper.ExternalScripts;
public static class ServiceCollectionsExtensions
{
    public static IServiceCollection AddDapperExternalScripts(this IServiceCollection services,
        Action<ScriptFinderGlobalConfiguration> configurationCallback)
    {
        services.AddSingleton(typeof(IScriptFinder<>), typeof(ScriptFinder<>));


        var cfg = new ScriptFinderGlobalConfiguration();
        configurationCallback(cfg);

        services.AddSingleton(cfg);

        return services;
    }
}