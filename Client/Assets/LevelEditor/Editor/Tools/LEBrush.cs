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
        // 笔刷资源
        private LEPrefabGo prefabGo;
        // 笔刷对应的物体
        private GameObject gameObject;
        // 笔刷当前位置
        private Vector3 position;
        // 是否可向场景中输入
        private bool enabledInput;
        // 当前状态
        private GizmoPanelState gpState;

        /// <summary>
        /// 创建 GameObject
        /// </summary>
        private void CreateGameObject()
        {
            Close();

            if (prefabGo != null)
            {
                gameObject = GameObject.Instantiate(prefabGo.go) as GameObject;
                gameObject.name = "brush";
                gameObject.isStatic = true;
                gameObject.hideFlags = HideFlags.HideAndDontSave;
            }
        }

        /// <summary>
        /// 设置笔刷 PrefabGo
        /// </summary>
        /// <param name="pgo"></param>
        public void SetPrefabGo(LEPrefabGo pgo)
        {
            prefabGo = pgo;

            CreateGameObject();
        }

        /// <summary>
        /// 清楚 prefabGo
        /// </summary>
        public void CleanPrefabGo()
        {
            prefabGo = null;
            enabledInput = false;
            Close();
        }
        
        public override void DrawScenePreview(SceneView sceneView, Vector3 mousePosition)
        {
            if (Event.current.alt == true || Event.current.shift == true || Event.current.control == true)
            {
                mousePosition = position;
            }
            
            if (prefabGo != null)
            {
                if (gameObject == null)
                {
                    enabledInput = false;
                }
                else {
                    enabledInput = true;

                    gameObject.transform.position = mousePosition;

                    if (position.x != mousePosition.x || position.y != mousePosition.y || position.z != mousePosition.z)
                    {
                        SceneView.RepaintAll();
                    }
                }
            }
            else {
                enabledInput = false;

                if (gpState != GizmoPanelState.Exit)
                {
                    LECubeGizmoGrid.DrawEmptyInk(mousePosition);
                }
            }

            position = mousePosition;
        }
        
        public override void HandleInput(Vector3 mousePosition)
        {
            if (Event.current.button == 0 && Event.current.alt == false &&Event.current.shift == false && Event.current.control == false
                && enabledInput)
            {
                if (LEWindow.Inst.currentSelectFunction == GridFunction.Ground)
                {
                    if (Event.current.type == EventType.MouseDrag || Event.current.type == EventType.MouseDown)
                    {
                        LELevel.Inst.Draw(prefabGo, position, Vector3.zero, LEWindow.Inst.currentSelectFunction, LEWindow.Inst.area.ToString());
                    }
                }
                else {
                    if (Event.current.type == EventType.MouseDown)
                    {
                        LELevel.Inst.Draw(prefabGo, position, Vector3.zero, LEWindow.Inst.currentSelectFunction, LEWindow.Inst.area.ToString());
                    }
                }
            }
        } 

        public override void HaneleGizmoPanelState(GizmoPanelState state)
        {
            gpState = state;

            if (state == GizmoPanelState.Exit)
            {
                Close();
            }
            else {
                CreateGameObject();
            }
        }

        public override void Close()
        {
            if (gameObject != null)
                GameObject.DestroyImmediate(gameObject);
            gameObject = null;
        }

        public override void Destroy()
        {
            prefabGo = null;
            if (gameObject != null)
                GameObject.DestroyImmediate(gameObject);
            gameObject = null;
        }
    }
}
