﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace SU.Editor.LevelEditor
{
    public class LECreateWindow : EditorWindow
    {
        static LECreateWindow window;

        // 关卡场景名称
        private string levelSceneName;

        // 关卡规模
        private Vector2Int dimension = new Vector2Int(20, 20);
        
        [MenuItem("SU/LevelEditor/Create Level")]
        public static void Initialize()
        {
            window = GetWindow<LECreateWindow>();
            window.titleContent.text = "Create Level";
        }

        void OnGUI()
        {
            if (Application.isPlaying)
                return;

            EditorGUILayout.Space();

            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.LabelField("LevelSceneName");
            levelSceneName = EditorGUILayout.TextField(levelSceneName);
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical("box");
            dimension = EditorGUILayout.Vector2IntField("Grid Dimensions", dimension);
            EditorGUILayout.EndVertical();

            if (GUILayout.Button("Create Level", GUILayout.Height(30)))
            {
                if (string.IsNullOrEmpty(levelSceneName))
                {
                    ShowNotification(new GUIContent("please input level scene name."));
                    return;
                }

                // 首字母转成大写
                levelSceneName = LEUtils.ToUpperFirstChar(levelSceneName);

                // 判断场景是否已经存在
                if (Directory.Exists(LEUtils.GetLevelSceneDirectory(levelSceneName)) || Directory.Exists(LEUtils.GetLevelScenePath(levelSceneName)))
                {
                    if (EditorUtility.DisplayDialog("Create Level", "The level scene already exists to create a new one?", "Create New", "Cancel"))
                    {
                        Directory.Delete(LEUtils.GetLevelSceneDirectory(LEUtils.GetLevelSceneDirectory(levelSceneName), true), true);

                        CreateLevel();
                    }
                }
                else {
                    CreateLevel();
                }
            }
        }

        /// <summary>
        /// 创建关卡
        /// </summary>
        private void CreateLevel()
        {
            if (!Directory.Exists(LEUtils.GetLevelSceneDirectory(levelSceneName)))
                Directory.CreateDirectory(LEUtils.GetLevelSceneDirectory(levelSceneName));

            // 创建空场景
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);

            // 创建网格
            GameObject gridPanel = GameObject.Find(LEConst.EditorGizmoGridPanelName);
            if (gridPanel != null)
            {
                DestroyImmediate(gridPanel);
            }
            gridPanel = new GameObject();
            gridPanel.layer = LayerMask.NameToLayer(LEConst.EditorGizmoGridLayerName);
            gridPanel.name = LEConst.EditorGizmoGridPanelName;
            LEGizmoPanel gizmoPanel = gridPanel.AddComponent<LEGizmoPanel>();
            gizmoPanel.SetGridSize(dimension.x, dimension.y);
            gizmoPanel.SetGridHight(0);

            // level
            GameObject levelGo = GameObject.Find(LEConst.EditorLevelName);
            if (levelGo != null)
            {
                DestroyImmediate(levelGo);
            }
            levelGo = new GameObject();
            levelGo.name = LEConst.EditorLevelName;
            var leLevel = levelGo.AddComponent<LELevel>();
            leLevel.levelSceneName = LEUtils.ToUpperFirstChar(levelSceneName);

            // layer
            var layerGo = new GameObject();
            layerGo.name = "Layers";
            layerGo.transform.parent = levelGo.transform;
            
            EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene(), LEUtils.GetLevelScenePath(levelSceneName));

            Close();

            LEWindow.Initialize();
        }
    }
}
