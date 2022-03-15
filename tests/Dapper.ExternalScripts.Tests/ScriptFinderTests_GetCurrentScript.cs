
using Dapper.ExternalScripts.Configuration;
using Dapper.ExternalScripts.Tests.ModelsToTest;

using Shouldly;

using System;
using System.IO;

using Xunit;

namespace Dapper.ExternalScripts.Tests;
public class ScriptFinderTests_GetCurrentScript
{
    [Fact]
    public void ScriptFinder_GetCurrentScript_Should_ThrowInvalidOperationException()
    {
        ScriptFinderGlobalConfiguration globalConfiguration = new ScriptFinderGlobalConfiguration();

        globalConfiguration.Configure<ThreeMethods>(x =>
        {
            x
                .SetRoute("SQLFiles/products/")
                .Rename("GetSingle", "GetOne");
        });


        var instance = new ScriptFinder<ThreeMethods>(globalConfiguration);
        Action act = () => instance.GetCurrentScript("random");

        act.ShouldThrow<InvalidOperationException>()
            .Message
            .ShouldContain("can't find method name");
    }

    [Fact]
    public void ScriptFinder_GetCurrentScript_Should_ReturnSuccessfully()
    {
        ScriptFinderGlobalConfiguration globalConfiguration = new ScriptFinderGlobalConfiguration();

        globalConfiguration.Configure<ThreeMethods>(x =>
        {
            x
                .SetRoute("SQLFiles/products/")
                .Rename("GetSingle", "GetOne");
        });


        var instance = new ScriptFinder<ThreeMethods>(globalConfiguration);
        var getAllScript = instance.GetCurrentScript(nameof(ThreeMethods.GetAll));
        getAllScript.ShouldBe(File.ReadAllText("SQLFiles\\Products\\GetAll.sql"));

        var getAllByTextScript = instance.GetCurrentScript(nameof(ThreeMethods.GetAllByText));
        getAllByTextScript.ShouldBe(File.ReadAllText("SQLFiles\\Products\\GetAllByText.sql"));
        var getOneTextScript = instance.GetCurrentScript(nameof(ThreeMethods.GetSingle));
        getOneTextScript.ShouldBe(File.ReadAllText("SQLFiles\\Products\\GetOne.sql"));
    }
}
