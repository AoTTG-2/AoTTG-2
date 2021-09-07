using System;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Assets.Scripts
{
    /// <summary>
    /// Used to format a branch name from <see cref="VersionManager"/>
    /// </summary>
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
            var regex = new Regex(issueRegex);
            var match = regex.Match(branchName);
            if (!match.Success)
                return branchName;

            var formatted = versionPattern;
            var names = regex.GetGroupNames();
            foreach (var name in names)
            {
                var group = match.Groups[name];
                formatted = formatted.Replace($"<{name}>", group.Value);
            }

            return formatted;
        }
    }
}