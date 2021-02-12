using Assets.Scripts;
using NUnit.Framework;
using System.Collections.Generic;

namespace Assets.Tests.EditorMode
{
    public class VersionFormatterTest
    {
        private readonly Dictionary<string, string> expectedByInput = new Dictionary<string, string>()
        {
            ["#157-gitignore-csproj"] = "Alpha-Issue#157",
            ["#164-cursor-overhaul"] = "Alpha-Issue#164",
            ["#176-editorconfig"] = "Alpha-Issue#176",
            ["#75-cannons"] = "Alpha-Issue#75",
            ["development"] = "development",
            ["master"] = "master",
            ["titan-fix"] = "titan-fix",
            ["version-manager"] = "version-manager"
        };

        /// <summary>
        /// This relies on the defaults of <see cref="VersionFormatter"/>,
        /// so it may break when that changes.
        /// </summary>
        [Test]
        public void DefaultFormatterHandlesDictionary()
        {
            var formatter = new VersionFormatter();
            RunDictionaryTest(formatter);
        }

        /// <summary>
        /// Dictionary was designed to handle this.
        /// It shouldn't break, unless the dictionary is modified.
        /// </summary>
        [Test]
        public void DictionaryTestBenchmark()
        {
            RunDictionaryTest(new VersionFormatter("#(?<issue>\\d+)", "Alpha-Issue#<issue>"));
        }

        private void RunDictionaryTest(VersionFormatter formatter)
        {
            foreach (var pair in expectedByInput)
            {
                var branchName = pair.Key;
                var expected = pair.Value;
                var actual = formatter.FormatBranchName(branchName);

                Assert.AreEqual(expected, actual);
            }
        }
    }
}