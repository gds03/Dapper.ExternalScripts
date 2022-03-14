﻿using AutoFixture;

using Dapper.ExternalScripts.Attributes;

using Shouldly;

using System;
using Xunit;

namespace Dapper.ExternalScripts.Tests.Attributes;
public class DapperRenameAttributeTests
{
    private readonly Fixture fixture = new();

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void DapperRenameAttribute_Ctor_Should_Throw_ArgumentException_When_Filename_NullOrEmpty(string filename)
    {
        //arrange & act
        Action act = () => new DapperRenameAttribute(filename);

        act.ShouldThrow<ArgumentException>().Message.ShouldBe("fileName");
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void DapperRenameAttribute_Ctor_Should_Throw_ArgumentException_When_FileExtension_NullOrEmpty(string fileExtension)
    {
        //arrange & act
        Action act = () => new DapperRenameAttribute(fixture.Create<string>(), fileExtension);

        act.ShouldThrow<ArgumentException>().Message.ShouldBe("fileExtension");
    }

    [Theory]
    [InlineData("filename 2")]
    public void DapperRenameAttribute_Ctor_Should_Throw_InvalidOperationException_When_FilenameHasEmptySpaces(string filename)
    {
        //arrange & act
        Action act = () => new DapperRenameAttribute(filename, fixture.Create<string>());

        act.ShouldThrow<InvalidOperationException>().Message.ShouldContain("can't contain spaces.");
    }

    [Theory]
    [InlineData("filename1\\")]
    [InlineData("filename1/")]
    public void DapperRenameAttribute_Ctor_Should_Throw_InvalidOperationException_When_FilenameHasSlashes(string filename)
    {
        //arrange & act
        Action act = () => new DapperRenameAttribute(filename, fixture.Create<string>());

        act.ShouldThrow<InvalidOperationException>().Message.ShouldBe("fileName can't contain any '\' or '/' characters");
    }

    [Theory]
    [InlineData(" s q l")]
    public void DapperRenameAttribute_Ctor_Should_Throw_InvalidOperationException_When_FileExtensionHasSlashes(string fileExtension)
    {
        //arrange & act
        Action act = () => new DapperRenameAttribute(fixture.Create<string>(), fileExtension);

        act.ShouldThrow<InvalidOperationException>().Message.ShouldBe("fileExtension can't contain spaces.");
    }
    [Theory]
    [InlineData("sql\\")]
    [InlineData("sql/")]
    [InlineData(".sql")]
    [InlineData("..sql")]
    [InlineData("sql.")]
    public void DapperRenameAttribute_Ctor_Should_Throw_InvalidOperationException_When_FileExtensionHasInvalidChars(string fileExtension)
    {
        //arrange & act
        Action act = () => new DapperRenameAttribute(fixture.Create<string>(), fileExtension);

        act.ShouldThrow<InvalidOperationException>().Message.ShouldBe("fileExtension can't contain any '\\' or '/' or '.' characters");
    }

    [Theory]
    [InlineData("filename", "sql")]
    [InlineData("query1", "sql")]
    [InlineData("query1", "mysql")]
    public void DapperRenameAttribute_Ctor_Should_Initialize_Successfully(string filename, string fileExtension)
    {
        //arrange & act
        Func<DapperRenameAttribute> act = () => new DapperRenameAttribute(filename, fileExtension);

        var value = act.ShouldNotThrow();

        value.FileName.ShouldBe(filename);
        value.FileExtension.ShouldBe(fileExtension);
    }
}
