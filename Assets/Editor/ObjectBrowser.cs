using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace Assets.Editor
{
    public class ObjectBrowser : EditorWindow
    {

        public List<Object> Objects = new List<Object>();
        Type objType;

        StyleSheet style;
        VisualTreeAsset layout;

        VisualElement scrollContent;
        Button btnAccept;
        ToolbarSearchField searchField;
        Label selectedLabel;

        VisualElement selected;
        int selectedIndex = -1;
        event Action<int> onSelectObject;

        public static void Open<T>(IEnumerable<T> objectArray, Action<int> indexCallback) where T : Object
        {

            var win = CreateInstance<ObjectBrowser>();

            win.Objects.AddRange(objectArray);
            win.objType = typeof(T);
            win.onSelectObject += indexCallback;
            win.ShowUtility();
            win.BuildUI();
        }

        private void BuildUI()
        {
            if (!style)
                style = Resources.Load<StyleSheet>("objectbrowser");
            if (!layout)
                layout = Resources.Load<VisualTreeAsset>("objectbrowser");

            var clone = layout.CloneTree();
            rootVisualElement.Add(clone);
            rootVisualElement.styleSheets.Add(style);
            clone.StretchToParentSize();

            scrollContent = rootVisualElement.Q("unity-content-container");
            selectedLabel = rootVisualElement.Q<Label>("selected");

            btnAccept = rootVisualElement.Q<Button>("btn-accept");
            btnAccept.clicked += BtnAccept_clicked;

            searchField = rootVisualElement.Q<ToolbarSearchField>("search_field");
            searchField.RegisterValueChangedCallback(Searched);


            for (var i = 0; i < Objects.Count; i++)
            {
                var obj = Objects[i];
                var index = i;

                if (obj is Texture2D texture)
                {
                    var v = new VisualElement();
                    v.AddToClassList("icon");
                    v.style.backgroundImage = new StyleBackground(texture);
                    v.name = texture.name;
                    v.userData = new Data { index = index, obj = obj };
                    v.tooltip = AssetDatabase.GetAssetPath(texture);
                    v.RegisterCallback<ClickEvent>(ClickedImage);
                    scrollContent.Add(v);

                    var lbl = new Label(texture.name);
                    v.Add(lbl);
                }

            }
        }

        private void Searched(ChangeEvent<string> evt)
        {
            foreach (var ve in scrollContent.Children())
            {
                bool show = evt.newValue == string.Empty | ve.name.ToLower().Contains(evt.newValue.ToLower());


                ve.style.display = new StyleEnum<DisplayStyle>(show ? DisplayStyle.Flex : DisplayStyle.None);
            }
        }

        class Data
        {
            public int index;
            public object obj;


            public override string ToString()
            {
                return $"[{index}] {((Object) obj).name}";
            }
        }

        private void ClickedImage(ClickEvent evt)
        {
            VisualElement ve = (VisualElement) evt.currentTarget;
            if (ve != null)
            {
                Data data = (Data) ve.userData;
                if (data != null)
                {
                    if (selected != null)
                    {
                        selected.RemoveFromClassList("selected");
                    }
                    selected = ve;
                    selectedIndex = data.index;
                    selected.AddToClassList("selected");
                    selectedLabel.text = data.ToString();
                }
            }
        }

        private void BtnAccept_clicked()
        {
            if (selectedIndex == -1)
                return;

            onSelectObject?.Invoke(selectedIndex);

            Close();
        }

        private void OnLostFocus()
        {
            Close();
        }
    }
}
