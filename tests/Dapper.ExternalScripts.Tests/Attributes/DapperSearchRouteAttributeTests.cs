using Dapper.ExternalScripts.Attributes;

using Shouldly;

using System;

using Xunit;

namespace Dapper.ExternalScripts.Tests.Attributes;
public class DapperSearchRouteAttributeTests
{
    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void DapperSearchRouteAttribute_Ctor_Should_Throw_ArgumentException_When_NullOrEmpty(string input)
    {
         //arrange & act
         Action act = () => new DapperSearchRouteAttribute(input);

        act.ShouldThrow<ArgumentException>().Message.ShouldBe("route");
    }

    [Theory]
    [InlineData("sql/folder 1")]
    public void DapperSearchRouteAttribute_Ctor_Should_Throw_InvalidOperationException_When_route_contains_whitespace(string input)
    {
        //arrange & act
        Action act = () => new DapperSearchRouteAttribute(input);

        act.ShouldThrow<InvalidOperationException>().Message.ShouldBe("route can't contain spaces.");
    }


    [Theory]
    [InlineData(@"sql\folder1")]
    public void DapperSearchRouteAttribute_Ctor_Should_Throw_InvalidOperationException_When_route_contains_anyBackwardSlash(string input)
    {
        //arrange & act
        Action act = () => new DapperSearchRouteAttribute(input);

        act.ShouldThrow<InvalidOperationException>().Message.ShouldBe("route can't contain any backward slash. Use forward slash");
    }

    [Theory]
    [InlineData(@"sql")]
    public void DapperSearchRouteAttribute_Ctor_Should_Throw_InvalidOperationException_When_route_contains_OnlyOneLevelFolder(string input)
    {
        //arrange & act
        Action act = () => new DapperSearchRouteAttribute(input);

        act.ShouldThrow<InvalidOperationException>().Message.ShouldBe("route must have at least one '/' to specify the first level folder");
    }

    [Theory]
    [InlineData(@"/sql/folder2")]
    public void DapperSearchRouteAttribute_Ctor_Should_Throw_InvalidOperationException_When_route_contains_firstcharSlash(string input)
    {
        //arrange & act
        Action act = () => new DapperSearchRouteAttribute(input);

        act.ShouldThrow<InvalidOperationException>().Message.ShouldBe("route first char must start with the directory name and not the '/' character");
    }

    [Theory]
    [InlineData("sql/folder2")]
    public void DapperSearchRouteAttribute_Ctor_Should_Throw_Initialize_Successfully(string input)
    {
        //arrange & act
        Func<DapperSearchRouteAttribute> act = () => new DapperSearchRouteAttribute(input);

        var value = act.ShouldNotThrow();

        value.Route.ShouldBe("sql/folder2");
    }
}
