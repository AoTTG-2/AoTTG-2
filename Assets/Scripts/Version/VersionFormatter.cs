using System;
using System.Text.RegularExpressions;
using UnityEngine;

[Serializable]
public sealed class VersionFormatter
{
    [SerializeField]
    private string branchRegex = "#(?<issue>\\d+)";

    [SerializeField]
    private string buildPathPattern = "$RELEASE_TYPE/Issue<issue> ($ARCHITECTURE-$BUILD)";

    [SerializeField]
    private string versionPattern = "Alpha-Issue<issue>";

    public VersionFormatter()
    {
    }

    public VersionFormatter(string issueRegex, string versionPattern)
    {
        this.branchRegex = issueRegex;
        this.versionPattern = versionPattern;
    }

    public Tokens TokenizeBranchName(string branchName)
    {
        var regex = new Regex(branchRegex);

        var match = regex.Match(branchName);
        if (!match.Success)
            return new Tokens()
            {
                Version = branchName,
                BuildPath = $"$RELEASE_TYPE/{branchName}_$ARCHITECTURE"
            };

        var buildPath = buildPathPattern;
        var version = versionPattern;
        var names = regex.GetGroupNames();
        foreach (var name in names)
        {
            var group = match.Groups[name];
            buildPath = buildPath.Replace($"<{name}>", group.Value);
            version = version.Replace($"<{name}>", group.Value);
        }

        return new Tokens()
        {
            BuildPath = buildPath,
            Version = version
        };
    }

    public struct Tokens
    {
        public string BuildPath;
        public string Version;
    }
}