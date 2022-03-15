using AutoFixture;

using Dapper.ExternalScripts.Configuration;
using Dapper.ExternalScripts.Tests.ModelsToTest;

using Shouldly;

using System;

using Xunit;


namespace Dapper.ExternalScripts.Tests.Configuration;
public partial class ScriptFinderGlobalConfigurationTests
{
    readonly Fixture fixture = new();
    [Fact]
    public void ScriptFinderTypeConfiguration_Configure_Should_Throw_InvalidOperationException()
    {

        ScriptFinderGlobalConfiguration globalConfiguration = new ScriptFinderGlobalConfiguration();

        Action act = () => globalConfiguration.Configure<OneMethod>(x =>
        {
            
        });

        act.ShouldThrow<InvalidOperationException>()
            .Message
            .ShouldContain("Invalid configuration. Errors are");
    }


    [Fact]
    public void ScriptFinderTypeConfiguration_Configure_Should_RunSuccessfully()
    {

        ScriptFinderGlobalConfiguration globalConfiguration = new ScriptFinderGlobalConfiguration();

        globalConfiguration.Configure<OneMethod>(x =>
        {
            x.SetRoute("features/");
        });

        1.ShouldBe(1);
    }

}
