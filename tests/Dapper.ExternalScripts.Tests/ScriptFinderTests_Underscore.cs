using AutoFixture;

using Shouldly;

using System;
using System.IO;

using Xunit;

using static Dapper.ExternalScripts.Tests.ScriptFinderTests_Ctor;

namespace Dapper.ExternalScripts.Tests;
public class ScriptFinderTests_Underscore
{
    private readonly Fixture fixture = new Fixture();

    [Fact]
    public void ScriptFinder_Underscore_Should_ThrowInvalidOperationException_When_MethodName_NotFound()
    {
        ScriptFinder<ProductQueries> scriptFinder = new ScriptFinder<ProductQueries>();

        var act = () => scriptFinder._(sql => fixture.Create<string>());

        act.ShouldThrow<InvalidOperationException>().Message.ShouldContain("can't find method name");
    }

    [Fact]
    public void ScriptFinder_Underscore_Should_ReturnSuccessfully()
    {
        ScriptFinder<ProductQueries> scriptFinder = new ScriptFinder<ProductQueries>();

        var text = () => scriptFinder._(sql => sql, nameof(ProductQueries.GetAll));

        text.ShouldNotThrow();
        text().ShouldBe(File.ReadAllText("SQLFiles\\Products\\GetAll.sql"));
    }
}
