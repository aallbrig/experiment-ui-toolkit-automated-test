using System;
using System.IO;
using Editor.Demo.Controllers;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Demo.EditorWindows
{
    public class TodoAppWindow : EditorWindow
    {
        private VisualTreeAsset _visualTree;
        private TodoAppController _controller;

        private string SelfPath =>
            Path.GetDirectoryName(AssetDatabase.GetAssetPath(MonoScript.FromScriptableObject(this)));

        private string SelfAssetPath => SelfPath.Substring(SelfPath.IndexOf("Assets", StringComparison.Ordinal));
        private string ParentPath => Directory.GetParent(SelfPath)?.FullName;
        private string ParentAssetPath => ParentPath.Substring(ParentPath.IndexOf("Assets", StringComparison.Ordinal));

        [MenuItem("Todo App/Show Window")]
        private static void ShowWindow()
        {
            var window = GetWindow<TodoAppWindow>();
            window.titleContent = new GUIContent("Todo App");
            window.Show();
        }
        private void OnEnable()
        {
            var viewPath = Path.Join(ParentAssetPath, "Views/Windows/TodoApp.uxml");
            _visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(viewPath);
            if (_visualTree == null)
            {
                Debug.LogError($"view asset path did not yield a visual tree asset ({viewPath})");
                throw new NullReferenceException($"view asset path did not yield a visual tree asset ({viewPath})");
            }
        }
        private void CreateGUI()
        {
            rootVisualElement.Clear();
            _visualTree.CloneTree(rootVisualElement);

            _controller = new TodoAppController(rootVisualElement);
            LoadData();
        }
        private void LoadData()
        {
            // todo: load todo app data
        }
    }
}
