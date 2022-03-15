using AutoFixture;

using Dapper.ExternalScripts.Configuration;
using Dapper.ExternalScripts.Tests.ModelsToTest;

using Shouldly;

using System;
using System.Linq;

using Xunit;

namespace Dapper.ExternalScripts.Tests.Configuration;
public partial class ScriptFinderTypeConfigurationTests
{
    [InlineData(null, "a", "a")]
    [InlineData("", "a", "a")]
    [InlineData("a", null, "a")]
    [InlineData("a", "", "a")]
    [Theory]
    public void ScriptFinderTypeConfiguration_Rename_Should_Throw_ArgumentException(string fromMethodName, string toFileName, string extension)
    {
        ScriptFinderTypeConfiguration<OneMethod> configuration = new ScriptFinderTypeConfiguration<OneMethod>();

        Action act = () => configuration.Rename(fromMethodName, toFileName, extension);

        act.ShouldThrow<ArgumentException>()
            .Message
            .ShouldContain("can't be empty")
        ;
    }

    [InlineData("method_no_match")]
    [Theory]
    public void ScriptFinderTypeConfiguration_Rename_Should_Throw_InvalidOperationException(string fromMethodName)
    {
        ScriptFinderTypeConfiguration<OneMethod> configuration = new ScriptFinderTypeConfiguration<OneMethod>();

        Action act = () => configuration.Rename(fromMethodName, fixture.Create<string>());

        act.ShouldThrow<InvalidOperationException>()
            .Message
            .ShouldContain("method do not exists")
        ;
    }

    [InlineData("GetAll", "GetAllFilename", "sql")]
    [Theory]
    public void ScriptFinderTypeConfiguration_Rename_Should_SetRenamedSuccessfully(string fromMethodName, string toFileName, string extension)
    {
        ScriptFinderTypeConfiguration<OneMethod> configuration = new ScriptFinderTypeConfiguration<OneMethod>();

        configuration.Rename(fromMethodName, toFileName, extension);

        var entry= configuration.MethodMaps.First(m => m.Key == fromMethodName).Value;
        
        entry.Extension.ShouldBe(extension);
        entry.RenamedName.ShouldBe(toFileName);
    }
}
