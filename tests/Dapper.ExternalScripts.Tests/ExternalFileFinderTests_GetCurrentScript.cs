using AutoFixture;

using Shouldly;

using System;
using System.IO;

using Xunit;

using static Dapper.ExternalScripts.Tests.ExternalFileFinderTests_Ctor;

namespace Dapper.ExternalScripts.Tests;
public class ExternalFileFinderTests_GetCurrentScript
{
    private readonly Fixture fixture = new Fixture();

    [Fact]
    public void ExternalFileFinder_GetCurrentScript_Should_ThrowInvalidOperationException_When_MethodName_NotFound()
    {
        ExternalFileFinder<ProductQueries> externalFileFinder = new ExternalFileFinder<ProductQueries>();

        var act = () => externalFileFinder.GetCurrentScript(fixture.Create<string>());

        act.ShouldThrow<InvalidOperationException>().Message.ShouldContain("can't find method name");
    }

    [Fact]
    public void ExternalFileFinder_GetCurrentScript_Should_ReturnSuccessfully()
    {
        ExternalFileFinder<ProductQueries> externalFileFinder = new ExternalFileFinder<ProductQueries>();

        var text = () => externalFileFinder.GetCurrentScript(nameof(ProductQueries.GetAll));

        text.ShouldNotThrow();
        text().ShouldBe(File.ReadAllText("SQLFiles\\Products\\GetAll.sql"));
    }
}
