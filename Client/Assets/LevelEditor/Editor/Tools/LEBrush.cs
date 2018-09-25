using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SU.Editor.LevelEditor
{
    /// <summary>
    /// 笔刷工具
    /// </summary>
    public class LEBrush : LEToolBase
    {
        // 笔刷模型配置
        private LERepositoryAsset repositroyPrefab;
        // 笔芯
        private GameObject ink;
        // 笔刷当前位置
        private Vector3 position;
        // 是否可向场景中输入
        private bool enabledInput;

        /// <summary>
        /// 设置笔刷模型
        /// </summary>
        /// <param name="_mc"></param>
        public void SetModel(LERepositoryAsset _mc)
        {
            repositroyPrefab = _mc;

            if (ink != null)
            {
                bool isActive = ink.activeSelf;
                GameObject.DestroyImmediate(ink);
                ink = GameObject.Instantiate(repositroyPrefab.asset) as GameObject;
                ink.name = "ink";
                ink.isStatic = true;
                ink.hideFlags = HideFlags.HideAndDontSave;
                ink.SetActive(isActive);
            }
        }
        
        public override void DrawScenePreview(SceneView sceneView, Vector3 mousePosition)
        {
            if (Event.current.alt == true || Event.current.shift == true || Event.current.control == true)
            {
                mousePosition = position;
            }
            
            if (repositroyPrefab != null)
            {
                if (ink == null)
                {
                    ink = GameObject.Instantiate(repositroyPrefab.asset) as GameObject;
                    ink.name = "ink";
                }

                if (!ink.activeSelf)
                    ink.SetActive(true);

                ink.transform.position = mousePosition;

                enabledInput = true;

                if (position.x != mousePosition.x || position.y != mousePosition.y || position.z != mousePosition.z)
                {
                    SceneView.RepaintAll();
                }
            }
            else {
                enabledInput = false;

                LECubeGizmoGrid.DrawEmptyInk(mousePosition);
            }

            position = mousePosition;
        }
        
        public override void HandleInput(Vector3 mousePosition)
        {
            if ((Event.current.type == EventType.MouseDrag || Event.current.type == EventType.MouseDown) 
                && Event.current.button == 0 && Event.current.alt == false &&Event.current.shift == false && Event.current.control == false
                && enabledInput)
            {
                LELevel.Inst.Draw(repositroyPrefab.repositoryName, repositroyPrefab.asset, repositroyPrefab.assetPath, repositroyPrefab.assetName, repositroyPrefab.bundleName, position, LEWindow.Inst.Layer);
            }
        }

        public override void Close()
        {
            if (ink != null)
            {
                ink.SetActive(false);
            }
        }

        public override void Destroy()
        {
            if (ink != null)
            {
                GameObject.DestroyImmediate(ink);
                ink = null;
            }
        }
    }
}
