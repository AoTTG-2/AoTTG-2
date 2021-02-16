using System;
using System.IO;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Assets.Scripts
{
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

        private static bool TryGetBranchName(ref string branchName)
        {
            try
            {
                var headText = File.ReadAllText(Directory.GetCurrentDirectory() + $"{Path.DirectorySeparatorChar}.git{Path.DirectorySeparatorChar}HEAD");
                branchName = headText.Substring(headText.IndexOf('#'));
                return true;
            }
            catch (Exception e)
            {
                Debug.LogWarning("Unable to retrieve the branch name");
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
}