using AutoFixture;

using Dapper.ExternalScripts.Attributes;

using Shouldly;

using System;
using Xunit;

namespace Dapper.ExternalScripts.Tests.Attributes;
public class ScriptRenameAttributeTests
{
    private readonly Fixture fixture = new();

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void ScriptRenameAttribute_Ctor_Should_Throw_ArgumentException_When_Filename_NullOrEmpty(string filename)
    {
        //arrange & act
        Action act = () => new ScriptRenameAttribute(filename);

        act.ShouldThrow<ArgumentException>().Message.ShouldBe("fileName");
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void ScriptRenameAttribute_Ctor_Should_Throw_ArgumentException_When_FileExtension_NullOrEmpty(string fileExtension)
    {
        //arrange & act
        Action act = () => new ScriptRenameAttribute(fixture.Create<string>(), fileExtension);

        act.ShouldThrow<ArgumentException>().Message.ShouldBe("fileExtension");
    }

    [Theory]
    [InlineData("filename 2")]
    public void ScriptRenameAttribute_Ctor_Should_Throw_InvalidOperationException_When_FilenameHasEmptySpaces(string filename)
    {
        //arrange & act
        Action act = () => new ScriptRenameAttribute(filename, fixture.Create<string>());

        act.ShouldThrow<InvalidOperationException>().Message.ShouldContain("can't contain spaces.");
    }

    [Theory]
    [InlineData("filename1\\")]
    [InlineData("filename1/")]
    public void ScriptRenameAttribute_Ctor_Should_Throw_InvalidOperationException_When_FilenameHasSlashes(string filename)
    {
        //arrange & act
        Action act = () => new ScriptRenameAttribute(filename, fixture.Create<string>());

        act.ShouldThrow<InvalidOperationException>().Message.ShouldBe("fileName can't contain any '\' or '/' characters");
    }

    [Theory]
    [InlineData(" s q l")]
    public void ScriptRenameAttribute_Ctor_Should_Throw_InvalidOperationException_When_FileExtensionHasSlashes(string fileExtension)
    {
        //arrange & act
        Action act = () => new ScriptRenameAttribute(fixture.Create<string>(), fileExtension);

        act.ShouldThrow<InvalidOperationException>().Message.ShouldBe("fileExtension can't contain spaces.");
    }
    [Theory]
    [InlineData("sql\\")]
    [InlineData("sql/")]
    [InlineData(".sql")]
    [InlineData("..sql")]
    [InlineData("sql.")]
    public void ScriptRenameAttribute_Ctor_Should_Throw_InvalidOperationException_When_FileExtensionHasInvalidChars(string fileExtension)
    {
        //arrange & act
        Action act = () => new ScriptRenameAttribute(fixture.Create<string>(), fileExtension);

        act.ShouldThrow<InvalidOperationException>().Message.ShouldBe("fileExtension can't contain any '\\' or '/' or '.' characters");
    }

    [Theory]
    [InlineData("filename", "sql")]
    [InlineData("query1", "sql")]
    [InlineData("query1", "mysql")]
    public void ScriptRenameAttribute_Ctor_Should_Initialize_Successfully(string filename, string fileExtension)
    {
        //arrange & act
        Func<ScriptRenameAttribute> act = () => new ScriptRenameAttribute(filename, fileExtension);

        var value = act.ShouldNotThrow();

        value.FileName.ShouldBe(filename);
        value.FileExtension.ShouldBe(fileExtension);
    }
}
