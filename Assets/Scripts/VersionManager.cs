using System;
using System.IO;
using UnityEngine;
using Debug = UnityEngine.Debug;

[CreateAssetMenu, ExecuteInEditMode]
public sealed class VersionManager : ScriptableObject
{
    [SerializeField]
    private VersionFormatter branchNameFormatter;

    [SerializeField]
    private bool useBranchName = true;

    [SerializeField]
    private string version = string.Empty;

    public string Version => version;

#if UNITY_EDITOR

    private void OnEnable()
    {
        UpdateVersion();
    }

    private bool TryGetBranchName(ref string branchName)
    {
        try
        {
            string head_text = System.IO.File.ReadAllText(Directory.GetCurrentDirectory() + "\\.git\\HEAD");
            branchName = head_text.Substring(head_text.IndexOf('#'));
            return true;
        }
        catch (Exception e)
        {
            Debug.Log("unable to retrive the branch name");
            Debug.LogException(e);
        }
        return false;
    }

    private void UpdateVersion()
    {
        if (useBranchName && TryGetBranchName(ref version))
            version = branchNameFormatter.FormatBranchName(version);
    }

#endif
}