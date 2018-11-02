using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SmallUniverse.GameEditor.LevelEditor
{
    public abstract class LEToolBase
    {
        /// <summary>
        /// 画场景预览显示
        /// </summary>
        /// <param name="mousePosition">  </param>
        public virtual void DrawScenePreview(SceneView sceneView, Vector3 mousePosition)
        {
        }

        /// <summary>
        /// 按键输入
        /// </summary>
        /// <param name="mousePosition"></param>
        public virtual void HandleInput(Vector3 mousePosition)
        {
        }

        /// <summary>
        /// GizmoPanelState
        /// </summary>
        public virtual void HaneleGizmoPanelState(GizmoPanelState state)
        {
        }

        /// <summary>
        /// 关闭
        /// </summary>
        public virtual void Close()
        {
        }

        /// <summary>
        /// 销毁笔刷
        /// </summary>
        public virtual void Destroy()
        {
        }
    }
}
