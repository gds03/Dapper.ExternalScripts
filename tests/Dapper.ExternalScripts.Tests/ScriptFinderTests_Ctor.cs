using Dapper.ExternalScripts.Configuration;
using Dapper.ExternalScripts.Tests.ModelsToTest;

using Shouldly;

using System;
using System.IO;
using System.Linq;

using Xunit;

namespace Dapper.ExternalScripts.Tests;
public class ScriptFinderTests_Ctor
{
    [Fact]
    public void ScriptFinder_Ctor_Should_Throw_InvalidOperationException_When_GlobalConfiguration_isNull()
    {
        Action act = () => new ScriptFinder<OneMethod>(null);

        act.ShouldThrow<InvalidOperationException>().Message.ShouldContain("There is no configuration for type");
    }

    [Fact]
    public void ScriptFinder_Ctor_Should_Throw_InvalidOperationException_When_GlobalConfiguration_HasNoSubtypeConfigured()
    {
        Action act = () => new ScriptFinder<OneMethod>(new ScriptFinderGlobalConfiguration());

        act.ShouldThrow<InvalidOperationException>().Message.ShouldContain("There is no configuration for type");
    }

    [Fact]
    public void ScriptFinder_Ctor_Should_Throw_DirectoryNotFoundException()
    {
        ScriptFinderGlobalConfiguration globalConfiguration = new ScriptFinderGlobalConfiguration();

        globalConfiguration.Configure<OneMethod>(x =>
        {
            x.SetRoute("SQLFiles/NOTFOUND/");
        });

        Action act = () => new ScriptFinder<OneMethod>(globalConfiguration);

        act.ShouldThrow<DirectoryNotFoundException>()
            .Message
            .ShouldContain("does not exists");
    }


    [Fact]
    public void ScriptFinder_Ctor_Should_Throw_FileNotFoundException()
    {
        ScriptFinderGlobalConfiguration globalConfiguration = new ScriptFinderGlobalConfiguration();

        globalConfiguration.Configure<OneMethod>(x =>
        {
            x
                .SetRoute("SQLFiles/products/")
                .SetExtension("GetAll", "mongo");
        });

        Action act = () => new ScriptFinder<OneMethod>(globalConfiguration);

        act.ShouldThrow<AggregateException>()
            .InnerExceptions
            .ToList()
            .Select(x => x.Message)
            .Any(msg => msg.Contains("not find file"))
            .ShouldBeTrue();
    }

    [Fact]
    public void ScriptFinder_Ctor_Should_InitializeSuccessfully()
    {
        ScriptFinderGlobalConfiguration globalConfiguration = new ScriptFinderGlobalConfiguration();

        globalConfiguration.Configure<OneMethod>(x =>
        {
            x.SetRoute("SQLFiles/products/");
        });

        var instance = new ScriptFinder<OneMethod>(globalConfiguration);

        instance.ShouldNotBeNull();
    }
}
