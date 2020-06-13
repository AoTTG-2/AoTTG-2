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

#if UNITY_EDITOR

    private string branchName;

    public bool BranchNameDirty { get; set; } = true;

    public string BuildPath { get; private set; }

#endif

    public string Version => version;

#if UNITY_EDITOR

    private void OnEnable()
    {
        UpdateBranch();
    }

    private void OnValidate()
    {
        UpdateTokens();
    }

    private void Reset()
    {
        UpdateBranch();
    }

    private bool TryGetBranchName()
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

    private void UpdateBranch()
    {
        var lastBranchName = branchName;
        if (useBranchName && TryGetBranchName())
        {
            UpdateTokens();

            if (branchName != lastBranchName)
                BranchNameDirty = true;
        }
    }

    private void UpdateTokens()
    {
        var tokens = branchNameFormatter.TokenizeBranchName(branchName);
        version = tokens.Version;
        BuildPath = tokens.BuildPath;
    }

#endif
}