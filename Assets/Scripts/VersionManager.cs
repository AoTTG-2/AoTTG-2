using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
using Debug = UnityEngine.Debug;

[CreateAssetMenu, ExecuteInEditMode]
public sealed class VersionManager : ScriptableObject
{
    [SerializeField]
    private string version = string.Empty;

    [SerializeField]
    private bool useBranchName = true;

    [SerializeField]
    private string issueRegex = "#(?<issue>\\d+)";

    [SerializeField]
    private string versionPattern = "Alpha-Issue<issue>";

    public string Version => version;

#if UNITY_EDITOR

    private string FormatBranchName(string branchName)
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

    private void OnEnable()
    {
        UpdateVersion();
    }

    private bool TryGetBranchName(ref string branchName)
    {
        try
        {
            var startInfo = new ProcessStartInfo("git.exe")
            {
                UseShellExecute = false,
                WorkingDirectory = Directory.GetCurrentDirectory(),
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                Arguments = "rev-parse --abbrev-ref HEAD"
            };

            using (var process = new Process
            {
                StartInfo = startInfo
            })
            {
                process.Start();
                branchName = process.StandardOutput.ReadLine();
                return true;
            }
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }

        return false;
    }

    private void UpdateVersion()
    {
        if (useBranchName && TryGetBranchName(ref version))
            version = FormatBranchName(version);
    }

#endif
}