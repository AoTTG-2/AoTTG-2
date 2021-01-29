using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEditor.Timeline;
using UnityEngine;

namespace UnityEditorInternal
{
    class BindingTreeViewDataSource : TreeViewDataSource
    {
        public const int RootID = int.MinValue;
        public const int GroupID = -1;

        AnimationClip m_Clip;
        CurveDataSource m_CurveDataSource;

        public BindingTreeViewDataSource(
            TreeViewController treeView, AnimationClip clip, CurveDataSource curveDataSource)
            : base(treeView)
        {
            m_Clip = clip;
            showRootItem = false;
            m_CurveDataSource = curveDataSource;
        }

        void SetupRootNodeSettings()
        {
            showRootItem = false;
            SetExpanded(RootID, true);
            SetExpanded(GroupID, true);
        }

        public static string GroupName(EditorCurveBinding binding)
        {
            string property = AnimationWindowUtility.NicifyPropertyGroupName(binding.type, binding.propertyName);
            if (!string.IsNullOrEmpty(binding.path))
            {
                property = binding.path + " : " + property;
            }

            int lastArrayIdx = property.LastIndexOf("Array.");
            if (lastArrayIdx != -1)
            {
                property = property.Substring(0, lastArrayIdx - 1);
            }
            return property;
        }

        static string PropertyName(EditorCurveBinding binding, string arrayPrefixToRemove = "")
        {
            string propertyName = AnimationWindowUtility.GetPropertyDisplayName(binding.propertyName);
            if (propertyName.Contains("Array"))
            {
                propertyName = propertyName.Replace("Array.", "");
                if (!string.IsNullOrWhiteSpace(arrayPrefixToRemove))
                    propertyName = propertyName.Replace(arrayPrefixToRemove, "");
                propertyName = propertyName.TrimStart('.');
            }
            return propertyName;
        }

        public override void FetchData()
        {
            if (m_Clip == null)
                return;

            var bindings = AnimationUtility.GetCurveBindings(m_Clip)
                .Union(AnimationUtility.GetObjectReferenceCurveBindings(m_Clip))
                .ToArray();

            var results = bindings.GroupBy(p => GroupName(p), p => p, (key, g) => new
            {
                parent = key,
                bindings = g.ToList()
            }).OrderBy(t =>
                {
                    //Force transform order first
                    if (t.parent == "Position") return -3;
                    if (t.parent == "Rotation") return -2;
                    if (t.parent == "Scale") return -1;
                    return 0;
                }).ThenBy(t => t.parent);

            m_RootItem = new CurveTreeViewNode(RootID, null, "root", null)
            {
                children = new List<TreeViewItem>(1)
            };

            if (results.Any())
            {
                var groupingNode = new CurveTreeViewNode(GroupID, m_RootItem, m_CurveDataSource.groupingName, bindings)
                {
                    children = new List<TreeViewItem>()
                };

                m_RootItem.children.Add(groupingNode);

                foreach (var r in results)
                {
                    FillMissingTransformCurves(r.parent, r.bindings);
                    var newNode = new CurveTreeViewNode(r.parent.GetHashCode(), groupingNode, r.parent, r.bindings.ToArray());
                    groupingNode.children.Add(newNode);
                    for (int b = 0; b < r.bindings.Count; b++)
                    {
                        if (newNode.children == null)
                            newNode.children = new List<TreeViewItem>();

                        var binding = r.bindings[b];
                        string propDisplayName = PropertyName(binding) + (binding.isPhantom ? " (Default Value)" : string.Empty);
                        var bindingNode = new CurveTreeViewNode(binding.GetHashCode(), newNode, propDisplayName, new[] { binding });
                        newNode.children.Add(bindingNode);
                    }
                }
                SetupRootNodeSettings();
            }

            m_NeedRefreshRows = true;
        }

        private void FillMissingTransformCurves(string parent, List<EditorCurveBinding> bindings)
        {
            if (!AnimationWindowUtility.IsActualTransformCurve(bindings[0]) || bindings.Count == 3)
                return;

            string prefixProperyName = bindings.First().propertyName.Split('.').First();
            if (bindings.FirstOrDefault(p => p.propertyName.EndsWith(".x")) == default)
            {
                var b = EditorCurveBinding.FloatCurve(string.Empty, typeof(Transform), prefixProperyName + ".x");
                b.isPhantom = true;
                bindings.Insert(0, b);
            }

            if (bindings.FirstOrDefault(p => p.propertyName.EndsWith(".y")) == default)
            {
                var b = EditorCurveBinding.FloatCurve(string.Empty, typeof(Transform), prefixProperyName + ".y");
                b.isPhantom = true;
                bindings.Insert(1, b);
            }

            if (bindings.FirstOrDefault(p => p.propertyName.EndsWith(".z")) == default)
            {
                var b = EditorCurveBinding.FloatCurve(string.Empty, typeof(Transform), prefixProperyName + ".z");
                b.isPhantom = true;
                bindings.Insert(2, b);
            }
        }

        public void UpdateData()
        {
            m_TreeView.ReloadData();
        }
    }

    class CurveTreeViewNode : TreeViewItem
    {
        EditorCurveBinding[] m_Bindings;

        public EditorCurveBinding[] bindings
        {
            get { return m_Bindings; }
        }

        public CurveTreeViewNode(int id, TreeViewItem parent, string displayName, EditorCurveBinding[] bindings)
            : base(id, parent != null ? parent.depth + 1 : -1, parent, displayName)
        {
            m_Bindings = bindings;
        }
    }
}
