using System;
using System.Text.RegularExpressions;
using UnityEngine;

[Serializable]
public sealed class VersionFormatter
{
    [SerializeField]
    private string issueRegex = "#(?<issue>\\d+)";

    [SerializeField]
    private string versionPattern = "Alpha-Issue#<issue>";

    public VersionFormatter()
    {
    }

    public VersionFormatter(string issueRegex, string versionPattern)
    {
        this.issueRegex = issueRegex;
        this.versionPattern = versionPattern;
    }

    public string FormatBranchName(string branchName)
    {
        Regex regex = new Regex(issueRegex);

        Match match = regex.Match(branchName);
        if (!match.Success)
            return branchName;

        string formatted = versionPattern;
        string[] names = regex.GetGroupNames();
        foreach (string name in names)
        {
            var group = match.Groups[name];
            formatted = formatted.Replace($"<{name}>", group.Value);
        }

        return formatted;
    }
}