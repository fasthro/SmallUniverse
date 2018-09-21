using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SU.Editor.LevelEditor
{
    public class LEGizmoPanel : MonoBehaviour
    {
        // 地块片的尺寸
        private float tileSize = 1;
        // 网格尺寸
        private int gridWidth = 40;
        public int GridWidth {
            get {
                return gridWidth;
            }
        }
        private int gridDepth = 40;
        public int GridDepth
        {
            get
            {
                return gridDepth;
            }
        }
        // 网格Y轴高度
        private int gridHeight = 0;
        public int GridHeight
        {
            get
            {
                return gridHeight;
            }
        }

        private Color normalColor = Color.white;
        private Color borderColor = Color.green;
        private Color fillColor = new Color(1, 0, 0, 0.5f);

        private float tileOffset;
        private float gridWidthOffset;
        private float gridDepthOffset;
        private float gridOffset;

        private Vector3 gridMin;
        private Vector3 gridMax;

        private BoxCollider gridCollider;
        private Vector3 gridColliderCenter;
        private Vector3 gridColliderSize;

        // 是否开启网格
        [HideInInspector]
        public bool GizmoGridEnabled = true;
        // 是否以网格中心点为原点
        [HideInInspector]
        public bool CentreGrid = true;

        /// <summary>
        /// 设置网格Height
        /// </summary>
        /// <param name="h">height</param>
        public void SetGridHight(int h)
        {
            gridHeight = h;
        }

        /// <summary>
        /// 设置网格尺寸
        /// </summary>
        /// <param name="w">size width</param>
        /// <param name="d">size depth </param>
        public void SetGridSize(int w, int d)
        {
            gridWidth = w;
            gridDepth = d;

            SetGridCollider();
        }

        /// <summary>
        /// 设置碰撞框
        /// </summary>
        private void SetGridCollider()
        {
            gridCollider = gameObject.GetComponent<BoxCollider>();
            if (gridCollider == null)
                gridCollider = gameObject.AddComponent<BoxCollider>();
            
            // center
            if (CentreGrid)
            {
                gridColliderCenter.x = 0 - tileOffset;
                gridColliderCenter.y = 0 + gridHeight - tileOffset;
                gridColliderCenter.z = 0 - tileOffset;
            }
            else
            {
                gridColliderCenter.x = 0 + gridWidth / 2 * tileSize - tileOffset;
                gridColliderCenter.y = 0 + gridHeight - tileOffset;
                gridColliderCenter.z = 0 + gridDepth / 2 * tileSize - tileOffset;
            }

            gridCollider.center = gridColliderCenter;

            // size
            gridColliderSize.x = gridWidth;
            gridColliderSize.y = 0.1f;
            gridColliderSize.z = gridDepth;
            gridCollider.size = gridColliderSize;
        }

        void OnDrawGizmos()
        {
            if (!GizmoGridEnabled)
                return;

            tileOffset = tileSize / 2.0f;
            if (CentreGrid)
            {
                gridWidthOffset = gridWidth * tileSize / 2;
                gridDepthOffset = gridDepth * tileSize / 2;
            }
            else {
                gridWidthOffset = 0;
                gridDepthOffset = 0;
            }

            gridMin.x = gameObject.transform.position.x - gridWidthOffset - tileOffset;
            gridMin.y = gameObject.transform.position.y + gridHeight - tileOffset - gridOffset;
            gridMin.z = gameObject.transform.position.z - gridDepthOffset - tileOffset;
            gridMax.x = gridMin.x + (tileSize * gridWidth);
            gridMax.z = gridMin.z + (tileSize * gridDepth);
            gridMax.y = gridMin.y;
            
            DrawBase();
            DrawGrid();
            DrawBorder();

            SetGridCollider();
        }

        // 画背板
        private void DrawBase()
        {
            if (CentreGrid)
            {
                Gizmos.DrawCube(new Vector3(gameObject.transform.position.x - tileOffset, gameObject.transform.position.y + gridHeight - tileOffset - gridOffset, gameObject.transform.position.z - tileOffset),
                    new Vector3((gridWidth * tileSize), 0.01f, (gridDepth * tileSize)));
            }
            else
            {
                Gizmos.DrawCube(new Vector3(gameObject.transform.position.x + (gridWidth / 2 * tileSize) - tileOffset,
                    gameObject.transform.position.y + gridHeight - tileOffset - gridOffset,
                    gameObject.transform.position.z + (gridDepth / 2 * tileSize) - tileOffset),
                    new Vector3((gridWidth * tileSize), 0.01f, (gridDepth * tileSize)));
            }
        }

        // 画网格
        private void DrawGrid()
        {
            Gizmos.color = normalColor;

            if (tileSize != 0)
            {
                for (float i = tileSize; i < (gridWidth * tileSize); i += tileSize)
                {
                    Gizmos.DrawLine(
                        new Vector3((float)i + gridMin.x, gridMin.y, gridMin.z),
                        new Vector3((float)i + gridMin.x, gridMin.y, gridMax.z)
                        );
                }
            }

            if (tileSize != 0)
            {
                for (float j = tileSize; j < (gridDepth * tileSize); j += tileSize)
                {
                    Gizmos.DrawLine(
                        new Vector3(gridMin.x, gridMin.y, j + gridMin.z),
                        new Vector3(gridMax.x, gridMin.y, j + gridMin.z)
                        );
                }
            }
        }

        // 画边
        private void DrawBorder()
        {
            Gizmos.color = borderColor;

            // left side
            Gizmos.DrawLine(new Vector3(gridMin.x, gridMin.y, gridMin.z), new Vector3(gridMin.x, gridMin.y, gridMax.z));

            //bottom
            Gizmos.DrawLine(new Vector3(gridMin.x, gridMin.y, gridMin.z), new Vector3(gridMax.x, gridMin.y, gridMin.z));

            // Right side
            Gizmos.DrawLine(new Vector3(gridMax.x, gridMin.y, gridMin.z), new Vector3(gridMax.x, gridMin.y, gridMax.z));

            //top
            Gizmos.DrawLine(new Vector3(gridMin.x, gridMin.y, gridMax.z), new Vector3(gridMax.x, gridMin.y, gridMax.z));
        }
    }
}
