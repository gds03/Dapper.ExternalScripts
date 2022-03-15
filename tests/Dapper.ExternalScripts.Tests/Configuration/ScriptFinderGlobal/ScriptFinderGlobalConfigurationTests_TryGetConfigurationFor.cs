using AutoFixture;

using Dapper.ExternalScripts.Configuration;
using Dapper.ExternalScripts.Tests.ModelsToTest;

using Shouldly;

using System;
using System.Collections.Generic;

using Xunit;


namespace Dapper.ExternalScripts.Tests.Configuration;
public partial class ScriptFinderGlobalConfigurationTests
{
    [Fact]
    public void ScriptFinderTypeConfiguration_TryGetConfigurationFor_Should_ReturnTrue()
    {
        // arrange
        ScriptFinderGlobalConfiguration globalConfiguration = new ScriptFinderGlobalConfiguration();
        globalConfiguration
            .GetType()
            .GetField("_mapConfigurations", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(globalConfiguration, new Dictionary<Type, object>
            {
                { typeof(OneMethod), new ScriptFinderTypeConfiguration<OneMethod>() }
            });

        var found = globalConfiguration.TryGetConfigurationFor<OneMethod>(out var result);
        found.ShouldBe(true);
    }

    [Fact]
    public void ScriptFinderTypeConfiguration_TryGetConfigurationFor_Should_ReturnFalse()
    {
        // arrange
        ScriptFinderGlobalConfiguration globalConfiguration = new ScriptFinderGlobalConfiguration();
        var found = globalConfiguration.TryGetConfigurationFor<ScriptFinderGlobalConfigurationTests>(out var result);

        found.ShouldBe(false);
    }
}
