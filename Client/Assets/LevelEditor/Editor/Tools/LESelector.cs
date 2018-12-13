using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SmallUniverse.GameEditor.LevelEditor
{
    /// <summary>
    /// 选择器工具
    /// </summary>
    public class LESelector : LEToolBase
    {
        public enum SelectorState
        {
            None,
            Selected,
        }

        // 选择器当前状态
        private SelectorState state = SelectorState.None;
        // 当前选中的位置
        private Vector3 position;
        // 当前选中的Grid
        private LEGrid grid;

        private GUIContent content;

        public override void DrawScenePreview(SceneView sceneView, Vector3 mousePosition)
        {
            if (state == SelectorState.Selected)
            {
                LECubeGizmoGrid.DrawSelector(position);

                DrawRotateToolbar(sceneView);
            }
            else
            {
                LECubeGizmoGrid.DrawSelector(mousePosition);
            }
        }

        /// <summary>
        /// 画旋转工具
        /// </summary>
        /// <param name="sceneView"></param>
        private void DrawRotateToolbar(SceneView sceneView)
        {
            Handles.BeginGUI();

            GUI.skin = EditorGUIUtility.GetBuiltinSkin(EditorSkin.Scene);

            var beginX = sceneView.position.width / 2 - (LESetting.SceneToolSize * 3 + LESetting.SceneTooIInterval * 2);


            // RotateX
            content = new GUIContent(LEWindow.IconConfig.GetIconTexture("iconRotateX"));
            if (GUI.Toggle(new Rect(beginX, LESetting.SceneTooIY, LESetting.SceneToolSize, LESetting.SceneToolSize), false, content, GUI.skin.button))
            {
                grid.RotateX();
            }

            // RotateY
            content = new GUIContent(LEWindow.IconConfig.GetIconTexture("iconRotateY"));
            if (GUI.Toggle(new Rect(beginX + LESetting.SceneToolSize + LESetting.SceneTooIInterval, LESetting.SceneTooIY, LESetting.SceneToolSize, LESetting.SceneToolSize), false, content, GUI.skin.button))
            {
                grid.RotateY();
            }

            // RotateZ
            content = new GUIContent(LEWindow.IconConfig.GetIconTexture("iconRotateZ"));
            if (GUI.Toggle(new Rect(beginX + (LESetting.SceneToolSize + LESetting.SceneTooIInterval) * 2, LESetting.SceneTooIY, LESetting.SceneToolSize, LESetting.SceneToolSize), false, content, GUI.skin.button))
            {
                grid.RotateZ();
            }

            // position
            GUI.BeginGroup(new Rect(beginX, LESetting.SceneTooIY + LESetting.SceneToolSize + 5f, LESetting.SceneToolSize * 3 + LESetting.SceneTooIInterval * 2, 40f));
            GUI.Box(new Rect(0, 0, LESetting.SceneToolSize * 3 + LESetting.SceneTooIInterval * 2, 40f), "position");
            GUI.Label(new Rect(20, 20, LESetting.SceneToolSize * 3 + LESetting.SceneTooIInterval * 2, 40f), string.Format("x:{0} y:{1}, z:{2}", position.x, position.y, position.z));
            GUI.EndGroup();

            Handles.EndGUI();
        }

        public override void HandleInput(Vector3 mousePosition)
        {
            if ((Event.current.type == EventType.MouseDown)
                && Event.current.button == 0 && Event.current.alt == false && Event.current.shift == false && Event.current.control == false)
            {
                if (state == SelectorState.None && state != SelectorState.Selected)
                {
                    grid = LELevel.Inst.GetGrid(mousePosition, LEWindow.Inst.area.ToString());
                    if (grid != null)
                    {
                        state = SelectorState.Selected;
                        position = mousePosition;
                    }
                }
                else
                {
                    state = SelectorState.None;
                    grid = null;
                }
            }
        }

        public override void Close()
        {
            state = SelectorState.None;
            grid = null;
        }
    }
}
