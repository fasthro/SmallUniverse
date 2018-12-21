using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace SmallUniverse.GameEditor.XmlEditor
{
    public class XmlToCSVSettingEditor : EditorWindow
    {
        private static XmlToCSVSettingEditor window;

        public static XmlToCSVSettingList Settings;

        private Color m_guiColor;

        [MenuItem("SmallUniverse/XmlToCSV/Setting Window")]
        public static void Initialize()
        {
            window = GetWindow<XmlToCSVSettingEditor>();
            window.titleContent.text = "Setting";
        }

        [MenuItem("SmallUniverse/XmlToCSV/Export All")]
        public static void ExportAll()
        {
            var path = "Assets/XmlToCSVEditor/Setting.asset";
            Settings = AssetDatabase.LoadAssetAtPath(path, typeof(XmlToCSVSettingList)) as XmlToCSVSettingList;
            if(Settings == null)
            {
                Settings = ScriptableObject.CreateInstance<XmlToCSVSettingList>();
                AssetDatabase.CreateAsset(Settings, path);
            }
            Settings.Initialize();

            for (int i = 0; i < Settings.settings.Count; i++)
            {
                Settings.settings[i].Export();
            }

            Debug.Log("CSV Export All Finished!");
        }

        void OnEnable()
        {
            var path = "Assets/XmlToCSVEditor/Setting.asset";
            Settings = AssetDatabase.LoadAssetAtPath(path, typeof(XmlToCSVSettingList)) as XmlToCSVSettingList;
            if(Settings == null)
            {
                Settings = ScriptableObject.CreateInstance<XmlToCSVSettingList>();
                AssetDatabase.CreateAsset(Settings, path);
            }
            Settings.Initialize();
        }

        void OnGUI()
        {
            if (window == null)
                return;

            for (int i = 0; i < Settings.settings.Count; i++)
            {
                var setting = Settings.settings[i];

                GUILayout.BeginHorizontal();
                if (GUILayout.Button(setting.name, GUILayout.Width(window.position.width - 60), GUILayout.Height(30)))
                {
                    setting.e_showing = !setting.e_showing;
                }
                if (GUILayout.Button("Export", GUILayout.Width(50), GUILayout.Height(30)))
                {
                    setting.Export();
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                    Debug.Log("CSV Export [" + setting.name + "] Finished!");
                }
                GUILayout.EndHorizontal();

                if (setting.e_showing)
                {
                    GUILayout.BeginVertical("box");

                    GUILayout.BeginVertical("box");
                    GUILayout.Label("Export CSV Path");
                    setting.csvPath = GUILayout.TextField(setting.csvPath);
                    GUILayout.EndVertical();

                    GUILayout.BeginVertical("box");
                    GUILayout.Label("Export Code Path");
                    setting.codePath = GUILayout.TextField(setting.codePath);
                    GUILayout.EndVertical();

                    setting.merge = GUILayout.Toggle(setting.merge, "merge");
                    GUILayout.EndVertical();
                }
            }

            if (GUILayout.Button("Export All", GUILayout.Width(window.position.width), GUILayout.Height(30)))
            {
                for (int i = 0; i < Settings.settings.Count; i++)
                {
                    Settings.settings[i].Export();
                }
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                Debug.Log("CSV Export All Finished!");
            }
        }
    }
}