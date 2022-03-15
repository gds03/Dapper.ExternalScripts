using Dapper.ExternalScripts;
using Dapper.ExternalScripts.Configuration;

using Microsoft.Extensions.DependencyInjection;

namespace Dapper.Conventions;
public static class ServiceCollectionsExtensions
{
    public static IServiceCollection AddDapperExternalScripts(this IServiceCollection services, 
        Action<ScriptFinderGlobalConfiguration> configurationCallback)
    {
        services.AddSingleton(typeof(IScriptFinder<>), typeof(ScriptFinder<>));


        var cfg = new ScriptFinderGlobalConfiguration();
        configurationCallback(cfg);

        services.AddSingleton<ScriptFinderGlobalConfiguration>(cfg);

        return services;
    }
}