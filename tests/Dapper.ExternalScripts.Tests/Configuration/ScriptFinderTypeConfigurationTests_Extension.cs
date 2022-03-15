using Dapper.ExternalScripts.Configuration;
using Dapper.ExternalScripts.Tests.ModelsToTest;

using Shouldly;

using System;
using System.Linq;

using Xunit;

namespace Dapper.ExternalScripts.Tests.Configuration;
public partial class ScriptFinderTypeConfigurationTests
{
    [InlineData(null, "abc")]
    [InlineData("", "abc")]
    [InlineData("abc", null)]
    [InlineData("abc", "")]
    [Theory]
    public void ScriptFinderTypeConfiguration_Extension_Should_Throw_ArgumentException(string fromMethodName, string extension)
    {
        ScriptFinderTypeConfiguration<OneMethod> configuration = new ScriptFinderTypeConfiguration<OneMethod>();

        Action act = () => configuration.SetExtension(fromMethodName, extension);

        act.ShouldThrow<ArgumentException>()
            .Message
            .ShouldContain("can't be empty")
        ;
    }

    [InlineData("method_not_exists", "abc")]
    [Theory]
    public void ScriptFinderTypeConfiguration_Extension_Should_Throw_InvalidOperationException(string fromMethodName, string extension)
    {
        ScriptFinderTypeConfiguration<OneMethod> configuration = new ScriptFinderTypeConfiguration<OneMethod>();

        Action act = () => configuration.SetExtension(fromMethodName, extension);

        act.ShouldThrow<InvalidOperationException>()
            .Message
            .ShouldContain("method do not exists")
        ;
    }

    [InlineData("GetAll", "mongo")]
    [Theory]
    public void ScriptFinderTypeConfiguration_Extension_Should_SetSucessfully(string fromMethodName, string extension)
    {
        ScriptFinderTypeConfiguration<OneMethod> configuration = new ScriptFinderTypeConfiguration<OneMethod>();

        configuration.SetExtension(fromMethodName, extension);

        configuration.MethodMaps.First(m => m.Key == fromMethodName).Value.Extension.ShouldBe(extension);
    }
}
