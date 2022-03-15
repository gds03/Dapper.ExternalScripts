using Dapper.ExternalScripts.Configuration;
using Dapper.ExternalScripts.Tests.ModelsToTest;

using Shouldly;

using System;

using Xunit;

namespace Dapper.ExternalScripts.Tests.Configuration;
public partial class ScriptFinderTypeConfigurationTests
{
    [InlineData(null)]
    [InlineData("")]
    [Theory]
    public void ScriptFinderTypeConfiguration_Route_Should_Throw_ArgumentException(string route)
    {
        ScriptFinderTypeConfiguration<OneMethod> configuration = new ScriptFinderTypeConfiguration<OneMethod>();

        Action act = () => configuration.SetRoute(route);

        act.ShouldThrow<ArgumentException>()
            .Message
            .ShouldContain("can't be empty")
        ;
    }

    [InlineData("/Features")]
    [InlineData("/Whatever")]
    [Theory]
    public void ScriptFinderTypeConfiguration_Route_Should_Set(string route)
    {
        ScriptFinderTypeConfiguration<OneMethod> configuration = new ScriptFinderTypeConfiguration<OneMethod>();

        configuration.SetRoute(route);

        configuration.Route.ShouldBe(route);
    }
}
