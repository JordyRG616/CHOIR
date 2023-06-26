using System;
using UnityEditor;
using UnityEngine;
using UnityEditor.UI;
using UnityEngine.UIElements;

namespace PluginTest.Editor
{
    public class GlobalWindow : EditorWindow
    {
        [MenuItem("PluginTest/Configuracoes Gerais")]
        public static void ShowMyEditor()
        {
            // This method is called when the user selects the menu item in the Editor
            EditorWindow wnd = GetWindow<GlobalWindow>();
            wnd.titleContent = new GUIContent("GlobalWindow");
        }
        
        public void CreateGUI()
        {
            rootVisualElement.Add(new Label("Teste"));
        }
    }
}