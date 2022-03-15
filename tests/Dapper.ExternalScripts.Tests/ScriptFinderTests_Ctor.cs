using Dapper.ExternalScripts.Attributes;

using Shouldly;

using System;
using System.IO;

using Xunit;

namespace Dapper.ExternalScripts.Tests;
public class ScriptFinderTests_Ctor
{
    //
    public class SomeQueriesWillNotWorkBecauseAreNOTAnnotated
    {
    }

    [Fact]
    public void ScriptFinder_Ctor_Should_Throw_InvalidOperationException_When_TypeIsntMarked_With_DapperSearchRouteAttribute()
    {
        Action act = () => new ScriptFinder<SomeQueriesWillNotWorkBecauseAreNOTAnnotated>();

        act.ShouldThrow<InvalidOperationException>().Message.ShouldContain("To use this service please Mark");
    }




    //
    [ScriptsRoute(@"invalidPath/")]
    public class SomeQueriesWillNotWorkBecauseDirectoryDontExists
    {
        public virtual string? GetAll() { return null; }
    }
    [Fact]
    public void ScriptFinder_Ctor_Should_Throw_DirectoryNotFoundException_When_DirectoryDontExists()
    {
        Action act = () => new ScriptFinder<SomeQueriesWillNotWorkBecauseDirectoryDontExists>();

        act.ShouldThrow<DirectoryNotFoundException>().Message.ShouldContain("does not exists");
    }





    //
    [ScriptsRoute(@"SQLFiles/Products")]
    public class ProductQueriesWithNoFilesForMethods
    {
        public virtual string? GetUnknownFile() { return null; }

    }
    [Fact]
    public void ScriptFinder_Ctor_Should_Throw_AggregateException_Due_FilesNotFound()
    {
        var act = () => new ScriptFinder<ProductQueriesWithNoFilesForMethods>();

        act.ShouldThrow<AggregateException>()
            .InnerExceptions
            .Count
            .ShouldBe(1);
    }


    //
    [ScriptsRoute(@"SQLFiles/Products")]
    public class ProductQueriesWithDuplicatedMethodNames
    {
        public virtual string? GetAll() { return null; }
        public virtual string? GetAll(string text) { return null; }

    }
    [Fact]
    public void ScriptFinder_Ctor_Should_Throw_AggregateException_Due_DuplicatedMethodNames()
    {
        var act = () => new ScriptFinder<ProductQueriesWithDuplicatedMethodNames>();

        act.ShouldThrow<AggregateException>()
            .InnerExceptions
            .Count
            .ShouldBe(1);
    }



    //
    [ScriptsRoute(@"SQLFiles/Products")]
    public class ProductQueriesRenamedMethod
    {
        [ScriptRename("GetAll")]
        public virtual string? GetProducts() { return null; }
    }
    [Fact]
    public void ScriptFinder_Ctor_Should_Initialize_Successfully_RenamingAttributeFindingTheScriptFile()
    {
        var act = () => new ScriptFinder<ProductQueriesRenamedMethod>();

        var fileFinder = act.ShouldNotThrow();

        fileFinder.ShouldNotBeNull();
    }



    //
    [ScriptsRoute(@"SQLFiles/Products")]
    public class ProductQueries
    {
        public virtual string? GetAll() { return null; }
    }
    [Fact]
    public void ScriptFinder_Ctor_Should_Initialize_Successfully()
    {
        var act = () => new ScriptFinder<ProductQueries>();

        var fileFinder = act.ShouldNotThrow();

        fileFinder.ShouldNotBeNull();
    }
}
