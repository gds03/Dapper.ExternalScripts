using Dapper.ExternalScripts.Attributes;

using Shouldly;

using System;
using System.IO;

using Xunit;

namespace Dapper.ExternalScripts.Tests;
public class ExternalFileFinderTests_Ctor
{
    //
    public class SomeQueriesWillNotWorkBecauseAreNOTAnnotated
    {
    }

    [Fact]
    public void ExternalFileFinder_Ctor_Should_Throw_InvalidOperationException_When_TypeIsntMarked_With_DapperSearchRouteAttribute()
    {
        Action act = () => new ExternalFileFinder<SomeQueriesWillNotWorkBecauseAreNOTAnnotated>();

        act.ShouldThrow<InvalidOperationException>().Message.ShouldContain("To use this service please Mark");
    }




    //
    [DapperSearchRoute(@"invalidPath/")]
    public class SomeQueriesWillNotWorkBecauseDirectoryDontExists
    {
        public virtual string? GetAll() { return null; }
    }
    [Fact]
    public void ExternalFileFinder_Ctor_Should_Throw_DirectoryNotFoundException_When_DirectoryDontExists()
    {
        Action act = () => new ExternalFileFinder<SomeQueriesWillNotWorkBecauseDirectoryDontExists>();

        act.ShouldThrow<DirectoryNotFoundException>().Message.ShouldContain("does not exists");
    }





    //
    [DapperSearchRoute(@"SQLFiles/Products")]
    public class ProductQueriesWithNoFilesForMethods
    {
        public virtual string? GetUnknownFile() { return null; }

    }
    [Fact]
    public void ExternalFileFinder_Ctor_Should_Throw_AggregateException_Due_FilesNotFound()
    {
        var act = () => new ExternalFileFinder<ProductQueriesWithNoFilesForMethods>();

        act.ShouldThrow<AggregateException>()
            .InnerExceptions
            .Count
            .ShouldBe(1);
    }


    //
    [DapperSearchRoute(@"SQLFiles/Products")]
    public class ProductQueriesWithDuplicatedMethodNames
    {
        public virtual string? GetAll() { return null; }
        public virtual string? GetAll(string text) { return null; }

    }
    [Fact]
    public void ExternalFileFinder_Ctor_Should_Throw_AggregateException_Due_DuplicatedMethodNames()
    {
        var act = () => new ExternalFileFinder<ProductQueriesWithDuplicatedMethodNames>();

        act.ShouldThrow<AggregateException>()
            .InnerExceptions
            .Count
            .ShouldBe(1);
    }



    //
    [DapperSearchRoute(@"SQLFiles/Products")]
    public class ProductQueriesRenamedMethod
    {
        [DapperRename("GetAll")]
        public virtual string? GetProducts() { return null; }
    }
    [Fact]
    public void ExternalFileFinder_Ctor_Should_Initialize_Successfully_RenamingAttributeFindingTheScriptFile()
    {
        var act = () => new ExternalFileFinder<ProductQueriesRenamedMethod>();

        var fileFinder = act.ShouldNotThrow();

        fileFinder.ShouldNotBeNull();
    }



    //
    [DapperSearchRoute(@"SQLFiles/Products")]
    public class ProductQueries
    {
        public virtual string? GetAll() { return null; }
    }
    [Fact]
    public void ExternalFileFinder_Ctor_Should_Initialize_Successfully()
    {
        var act = () => new ExternalFileFinder<ProductQueries>();

        var fileFinder = act.ShouldNotThrow();

        fileFinder.ShouldNotBeNull();
    }
}
