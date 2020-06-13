using System;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using Debug = UnityEngine.Debug;

[CreateAssetMenu, ExecuteInEditMode]
public sealed class VersionManager : ScriptableObject
{
    [SerializeField]
    private bool useBranchName = true;

    [SerializeField]
    private VersionFormatter branchNameFormatter;

    [SerializeField]
    private string version = string.Empty;

    public string BuildPath { get; private set; }

    public string Version => version;

    private void Reset()
    {
        UpdateVersion();
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
        {
            var tokens = branchNameFormatter.TokenizeBranchName(version);
            version = tokens.Version;
            BuildPath = tokens.BuildPath;
        }
    }
}