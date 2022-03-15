namespace Dapper.ExternalScripts.Configuration;
public class ScriptFinderGlobalConfiguration
{
    private readonly Dictionary<Type, object> _mapConfigurations = new();

    public ScriptFinderGlobalConfiguration Configure<TSource>(Action<ScriptFinderTypeConfiguration<TSource>> source)
    {
        ScriptFinderTypeConfiguration<TSource> config = new ScriptFinderTypeConfiguration<TSource>();
        source(config);
        _mapConfigurations.Add(typeof(TSource), config);
        return this;
    }

    public bool TryGetConfigurationFor<TSource>(out ScriptFinderTypeConfiguration<TSource>? configuration)
    {
        configuration = default;
        if (_mapConfigurations.TryGetValue(typeof(TSource), out var config))
        {
            configuration = (ScriptFinderTypeConfiguration<TSource>)config;
            return true;
        }

        return false;
    }
}
