using AutoFixture;

using Dapper.ExternalScripts.Configuration;
using Dapper.ExternalScripts.Tests.ModelsToTest;

using Shouldly;

using Xunit;

namespace Dapper.ExternalScripts.Tests.Configuration;
public partial class ScriptFinderTypeConfigurationTests
{
    readonly Fixture fixture = new();

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

        configuration.MethodMaps.Count.ShouldBe(2);
    }
}
