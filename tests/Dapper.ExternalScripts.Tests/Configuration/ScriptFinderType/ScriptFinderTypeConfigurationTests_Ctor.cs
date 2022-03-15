using AutoFixture;

using Dapper.ExternalScripts.Configuration;
using Dapper.ExternalScripts.Tests.ModelsToTest;

using Shouldly;

using System;

using Xunit;

namespace Dapper.ExternalScripts.Tests.Configuration;
public partial class ScriptFinderTypeConfigurationTests
{
    readonly Fixture fixture = new();

    [Fact]
    public void ScriptFinderTypeConfiguration_Ctor_Should_Throw_InvalidOperationException_UnsupportedMethodOverload()
    {
        Action act = () => new ScriptFinderTypeConfiguration<FourMethods>();

        act.ShouldThrow<InvalidOperationException>()
           .Message
           .ShouldContain("Methods overloading are not supported in this version.");
    }

    [Fact]
    public void ScriptFinderTypeConfiguration_Ctor_Should_Map1Member()
    {
        ScriptFinderTypeConfiguration<OneMethod> configuration = new ScriptFinderTypeConfiguration<OneMethod>();

        configuration.MethodMaps.Count.ShouldBe(1);
    }
    [Fact]
    public void ScriptFinderTypeConfiguration_Ctor_Should_Map2Members()
    {
        ScriptFinderTypeConfiguration<TwoMethods> configuration = new ScriptFinderTypeConfiguration<TwoMethods>();

        configuration.MethodMaps.Count.ShouldBe(2);
    }

    [Fact]
    public void ScriptFinderTypeConfiguration_Ctor_Should_Map2Members_BecauseNameAreEqual()
    {
        ScriptFinderTypeConfiguration<ThreeMethods> configuration = new ScriptFinderTypeConfiguration<ThreeMethods>();

        configuration.MethodMaps.Count.ShouldBe(3);
    }
}
