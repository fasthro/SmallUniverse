using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SmallUniverse.GameEditor.LevelEditor
{
    public class LECubeGizmoGrid : EditorWindow
    {
        /// <summary>
        /// 笔刷工具没有笔芯
        /// </summary>
        /// <param name="monsePosition"></param>
        public static void DrawEmptyInk(Vector3 monsePosition)
        {
            DrawGizmoCube(monsePosition, Vector3.one, Color.red);
        }

        /// <summary>
        /// 选择器工具
        /// </summary>
        /// <param name="monsePosition"></param>
        public static void DrawSelector(Vector3 monsePosition)
        {
            DrawGizmoCube(monsePosition, Vector3.one, Color.yellow);
        }

        /// <summary>
        /// 擦除工具
        /// </summary>
        /// <param name="monsePosition"></param>
        public static void DrawErase(Vector3 monsePosition)
        {
            DrawGizmoCube(monsePosition, Vector3.one, Color.red);
        }

        /// <summary>
        /// 吸管工具
        /// </summary>
        /// <param name="monsePosition"></param>
        public static void DrawSucker(Vector3 monsePosition)
        {
            DrawGizmoCube(monsePosition, Vector3.one, Color.green);
        }

        

        /// <summary>
        /// 画 cube gizmo grid
        /// </summary>
        /// <param name="position"></param>
        /// <param name="size"></param>
        /// <param name="gizmoColor"></param>
        public static void DrawGizmoCube(Vector3 position, Vector3 size, Color gizmoColor)
        {
            Handles.color = gizmoColor;

            var full = size * 1.0f;
            var half = full / 2;
            var scale = 0.5f;

            // draw front
            Handles.DrawLine(position + new Vector3(-half.x, -scale, half.z), position + new Vector3(half.x, -scale, half.z));
            Handles.DrawLine(position + new Vector3(-half.x, -scale, half.z), position + new Vector3(-half.x, full.y - scale, half.z));
            Handles.DrawLine(position + new Vector3(half.x, full.y - scale, half.z), position + new Vector3(half.x, -scale, half.z));
            Handles.DrawLine(position + new Vector3(half.x, full.y - scale, half.z), position + new Vector3(-half.x, full.y - scale, half.z));

            // draw back
            Handles.DrawLine(position + new Vector3(-half.x, -scale, -half.z), position + new Vector3(half.x, -scale, -half.z));
            Handles.DrawLine(position + new Vector3(-half.x, -scale, -half.z), position + new Vector3(-half.x, full.y - scale, -half.z));
            Handles.DrawLine(position + new Vector3(half.x, full.y - scale, -half.z), position + new Vector3(half.x, -scale, -half.z));
            Handles.DrawLine(position + new Vector3(half.x, full.y - scale, -half.z), position + new Vector3(-half.x, full.y - scale, -half.z));

            // draw corners
            Handles.DrawLine(position + new Vector3(-half.x, -scale, -half.z), position + new Vector3(-half.x, -scale, half.z));
            Handles.DrawLine(position + new Vector3(half.x, -scale, -half.z), position + new Vector3(half.x, -scale, half.z));
            Handles.DrawLine(position + new Vector3(-half.x, full.y - scale, -half.z), position + new Vector3(-half.x, full.y - scale, half.z));
            Handles.DrawLine(position + new Vector3(half.x, full.y - scale, -half.z), position + new Vector3(half.x, full.y - scale, half.z));
            
            SceneView.RepaintAll();
        }
    }
}
