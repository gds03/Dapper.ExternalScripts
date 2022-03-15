//using Shouldly;

//using System;
//using System.IO;

//using Xunit;

//namespace Dapper.ExternalScripts.Tests;
//public class ScriptFinderTests_Ctor
//{
//    //
    

//    [Fact]
//    public void ScriptFinder_Ctor_Should_Throw_InvalidOperationException_When_TypeIsntMarked_With_DapperSearchRouteAttribute()
//    {
//        Action act = () => new ScriptFinder<SomeQueriesWillNotWorkBecauseAreNOTAnnotated>();

//        act.ShouldThrow<InvalidOperationException>().Message.ShouldContain("To use this service please Mark");
//    }




//    //

//    [Fact]
//    public void ScriptFinder_Ctor_Should_Throw_DirectoryNotFoundException_When_DirectoryDontExists()
//    {
//        Action act = () => new ScriptFinder<SomeQueriesWillNotWorkBecauseDirectoryDontExists>();

//        act.ShouldThrow<DirectoryNotFoundException>().Message.ShouldContain("does not exists");
//    }





//    //

//    [Fact]
//    public void ScriptFinder_Ctor_Should_Throw_AggregateException_Due_FilesNotFound()
//    {
//        var act = () => new ScriptFinder<ProductQueriesWithNoFilesForMethods>();

//        act.ShouldThrow<AggregateException>()
//            .InnerExceptions
//            .Count
//            .ShouldBe(1);
//    }


//    //

//    [Fact]
//    public void ScriptFinder_Ctor_Should_Throw_AggregateException_Due_DuplicatedMethodNames()
//    {
//        var act = () => new ScriptFinder<ProductQueriesWithDuplicatedMethodNames>();

//        act.ShouldThrow<AggregateException>()
//            .InnerExceptions
//            .Count
//            .ShouldBe(1);
//    }



//    //
 
//    [Fact]
//    public void ScriptFinder_Ctor_Should_Initialize_Successfully_RenamingAttributeFindingTheScriptFile()
//    {
//        var act = () => new ScriptFinder<ProductQueriesRenamedMethod>();

//        var fileFinder = act.ShouldNotThrow();

//        fileFinder.ShouldNotBeNull();
//    }



//    //
 
//    [Fact]
//    public void ScriptFinder_Ctor_Should_Initialize_Successfully()
//    {
//        var act = () => new ScriptFinder<ProductQueries>();

//        var fileFinder = act.ShouldNotThrow();

//        fileFinder.ShouldNotBeNull();
//    }
//}
