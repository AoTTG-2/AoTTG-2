using NUnit.Framework;
using System.Collections.Generic;

public class VersionFormatterTest
{
    private readonly Dictionary<string, string> expectedByInput = new Dictionary<string, string>()
    {
        ["#157-gitignore-csproj"] = "Alpha-Issue157",
        ["#164-cursor-overhaul"] = "Alpha-Issue164",
        ["#176-editorconfig"] = "Alpha-Issue176",
        ["#75-cannons"] = "Alpha-Issue75",
        ["development"] = "development",
        ["master"] = "master",
        ["titanfix"] = "titanfix",
        ["version-manager"] = "version-manager"
    };

    [Test]
    public void DefaultFormatterHandlesDictionary()
    {
        var formatter = new VersionFormatter();

        foreach (var pair in expectedByInput)
        {
            var branchName = pair.Key;
            var expected = pair.Value;
            var actual = formatter.FormatBranchName(branchName);

            Assert.AreEqual(expected, actual);
        }
    }
}