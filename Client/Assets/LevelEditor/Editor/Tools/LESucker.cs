using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
namespace SU.Editor.LevelEditor
{
    /// <summary>
    /// 吸管工具
    /// </summary>
    public class LESucker : LEToolBase
    {
        public override void DrawScenePreview(SceneView sceneView, Vector3 mousePosition)
        {
            LECubeGizmoGrid.DrawSucker(mousePosition);
        }

        public override void HandleInput(Vector3 mousePosition)
        {
            if ((Event.current.type == EventType.MouseDown)
               && Event.current.button == 0 && Event.current.alt == false && Event.current.shift == false && Event.current.control == false)
            {
                var grid = LELevel.Inst.GetGrid(LELevel.GetKey(mousePosition, LEWindow.Inst.GridGroud));
                if (grid != null)
                {
                    var rp = LEWindow.RepositoryConfig.GetRepositoryPrefab(grid.repositoryName, grid.assetPath);
                    if (rp != null)
                    {
                        LEWindow.Inst.brush.SetModel(rp);
                        LEWindow.Inst.ChangeSceneTool(SceneTools.Brush);
                    }
                    else {
                        Debug.LogError("There is no " + grid.assetPath + " in the " + grid.repositoryName);
                    }
                }
            }
        }
    }
}

