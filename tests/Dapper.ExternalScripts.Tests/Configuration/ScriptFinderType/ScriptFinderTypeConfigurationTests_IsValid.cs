using Dapper.ExternalScripts.Configuration;
using Dapper.ExternalScripts.Tests.ModelsToTest;

using Shouldly;

using System;

using Xunit;

namespace Dapper.ExternalScripts.Tests.Configuration;
public partial class ScriptFinderTypeConfigurationTests
{
    [Fact]
    public void ScriptFinderTypeConfiguration_Isvalid_Should_ReturnErrors()
    {
        ScriptFinderTypeConfiguration<OneMethod> configuration = new ScriptFinderTypeConfiguration<OneMethod>();

        bool isValid = configuration.IsValid(out string errorMsg);

        isValid.ShouldBe(false);
        errorMsg.ShouldNotBe(string.Empty);
    }

    [InlineData("abc def", "sql")]
    [InlineData("filename1/", "sql")]
    [InlineData("filename2\\", "sql")]
    [InlineData("filename3", "sql")]

    [InlineData("abcdef", "sql def")]
    [InlineData("abcdef", ".sql")]
    [InlineData("abcdef", "sql.")]
    [Theory]
    public void ScriptFinderTypeConfiguration_Isvalid_Should_ReturnErrors2(string renamedSample, string extension)
    {
        ScriptFinderTypeConfiguration<OneMethod> configuration = new ScriptFinderTypeConfiguration<OneMethod>();

        configuration.Rename(nameof(OneMethod.GetAll), renamedSample, extension);

        bool isValid = configuration.IsValid(out string errorMsg);

        isValid.ShouldBe(false);
        errorMsg.ShouldNotBe(string.Empty);
    }
}
