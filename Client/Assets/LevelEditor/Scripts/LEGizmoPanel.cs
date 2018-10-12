using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SU.Editor.LevelEditor
{
    public class LEGizmoPanel : MonoBehaviour
    {
        // 地块片的尺寸
        private float tileSize = 1;

        // 网格尺寸宽(X轴)
        private int _width = 40;
        public int width {
            get {
                return _width;
            }
        }

        // 网格尺寸长(Z轴)
        private int _lenght = 40;
        public int lenght
        {
            get
            {
                return _lenght;
            }
        }

        // 网格高度(Y轴)
        private int _height = 0;
        public int height
        {
            get
            {
                return _height;
            }
        }

        private Color normalColor = Color.white;
        private Color borderColor = Color.green;
        private Color fillColor = new Color(1, 0, 0, 0.5f);

        private float tileOffset;
        private float gridWidthOffset;
        private float gridLengthOffset;
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
        // 是否显示背景
        [HideInInspector]
        public bool BaseEnabled = true;


        /// <summary>
        /// 设置网格Height
        /// </summary>
        /// <param name="h">height</param>
        public void SetHight(int h)
        {
            _height = h;
        }

        /// <summary>
        /// 设置网格尺寸
        /// </summary>
        /// <param name="w">size width</param>
        /// <param name="d">size depth </param>
        public void SetSize(int w, int d)
        {
            _width = w;
            _lenght = d;

            SetCollider();
        }

        /// <summary>
        /// 设置碰撞框
        /// </summary>
        private void SetCollider()
        {
            gridCollider = gameObject.GetComponent<BoxCollider>();
            if (gridCollider == null)
                gridCollider = gameObject.AddComponent<BoxCollider>();
            
            // center
            if (CentreGrid)
            {
                gridColliderCenter.x = 0 - tileOffset;
                gridColliderCenter.y = 0 + _height - tileOffset;
                gridColliderCenter.z = 0 - tileOffset;
            }
            else
            {
                gridColliderCenter.x = 0 + _width / 2 * tileSize - tileOffset;
                gridColliderCenter.y = 0 + _height - tileOffset;
                gridColliderCenter.z = 0 + _lenght / 2 * tileSize - tileOffset;
            }

            gridCollider.center = gridColliderCenter;

            // size
            gridColliderSize.x = _width;
            gridColliderSize.y = 0.1f;
            gridColliderSize.z = _lenght;
            gridCollider.size = gridColliderSize;
        }

        void OnDrawGizmos()
        {
            if (!GizmoGridEnabled)
                return;

            tileOffset = tileSize / 2.0f;
            if (CentreGrid)
            {
                gridWidthOffset = _width * tileSize / 2;
                gridLengthOffset = _lenght * tileSize / 2;
            }
            else {
                gridWidthOffset = 0;
                gridLengthOffset = 0;
            }

            gridMin.x = gameObject.transform.position.x - gridWidthOffset - tileOffset;
            gridMin.y = gameObject.transform.position.y + _height - tileOffset - gridOffset;
            gridMin.z = gameObject.transform.position.z - gridLengthOffset - tileOffset;
            gridMax.x = gridMin.x + (tileSize * _width);
            gridMax.z = gridMin.z + (tileSize * _lenght);
            gridMax.y = gridMin.y;
            
            DrawBase();
            DrawGrid();
            DrawBorder();

            SetCollider();
        }

        // 画背板
        private void DrawBase()
        {
            if (!BaseEnabled)
                return;

            if (CentreGrid)
            {
                Gizmos.DrawCube(new Vector3(gameObject.transform.position.x - tileOffset, gameObject.transform.position.y + _height - tileOffset - gridOffset, gameObject.transform.position.z - tileOffset),
                    new Vector3((_width * tileSize), 0.01f, (_lenght * tileSize)));
            }
            else
            {
                Gizmos.DrawCube(new Vector3(gameObject.transform.position.x + (_width / 2 * tileSize) - tileOffset,
                    gameObject.transform.position.y + _height - tileOffset - gridOffset,
                    gameObject.transform.position.z + (_lenght / 2 * tileSize) - tileOffset),
                    new Vector3((_width * tileSize), 0.01f, (_lenght * tileSize)));
            }
        }

        // 画网格
        private void DrawGrid()
        {
            Gizmos.color = normalColor;

            if (tileSize != 0)
            {
                for (float i = tileSize; i < (_width * tileSize); i += tileSize)
                {
                    Gizmos.DrawLine(
                        new Vector3((float)i + gridMin.x, gridMin.y, gridMin.z),
                        new Vector3((float)i + gridMin.x, gridMin.y, gridMax.z)
                        );
                }
            }

            if (tileSize != 0)
            {
                for (float j = tileSize; j < (_lenght * tileSize); j += tileSize)
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
