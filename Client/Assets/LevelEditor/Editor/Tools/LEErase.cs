using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SU.Editor.LevelEditor
{
    /// <summary>
    /// 擦除工具
    /// </summary>
    public class LEErase : LEToolBase
    {
        public override void DrawScenePreview(SceneView sceneView, Vector3 mousePosition)
        {
            LECubeGizmoGrid.DrawErase(mousePosition);
        }

        public override void HandleInput(Vector3 mousePosition)
        {
            if ((Event.current.type == EventType.MouseDrag || Event.current.type == EventType.MouseDown)
               && Event.current.button == 0 && Event.current.alt == false && Event.current.shift == false && Event.current.control == false)
            {
                LELevel.Inst.Erase(mousePosition, LEWindow.Inst.area.ToString());
            }
        }
    }
}
