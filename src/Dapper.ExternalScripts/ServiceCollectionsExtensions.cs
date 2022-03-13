using Dapper.ExternalScripts;

using Microsoft.Extensions.DependencyInjection;

namespace Dapper.Conventions;
public static class ServiceCollectionsExtensions
{
    public static IServiceCollection AddDapperExternalScripts(this IServiceCollection services) =>
        services.AddSingleton(typeof(IExternalFileFinder<>), typeof(ExternalFileFinder<>));
}