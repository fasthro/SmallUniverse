using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace SmallUniverse.GameEditor.LevelEditor
{
    [ExecuteInEditMode]
    public class LEBoxLine : MonoBehaviour
    {
        // 偏移
        public Vector3 offset = Vector3.zero;
        // 大小
        public Vector3 size = Vector3.one;
        public Color32 color = Color.green;
        // 是一直显示
        public bool permanentDrawing = false;
        // line 材质
        private Material m_lineMaterial;

        Vector3 p0, p1, p2, p3;
        Vector3 p4, p5, p6, p7;

        private bool m_drawing;

        void OnEnable()
        {
            CreateLineMaterial();

            if (permanentDrawing)
            {
                m_drawing = true;
            }
        }

        public void Draw(bool drawing)
        {
            if (permanentDrawing)
                return;

            m_drawing = drawing;
        }

        public void SetColor(Color color)
        {
            if (permanentDrawing)
                return;

            this.color = color;
        }

        private void CreateLineMaterial()
        {
            if (m_lineMaterial == null)
            {
                m_lineMaterial = new Material(Shader.Find("Hidden/Internal-Colored"));
                m_lineMaterial.hideFlags = HideFlags.HideAndDontSave;
                // 开启 alpha blending    
                m_lineMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                m_lineMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                // 开启背面遮挡    
                m_lineMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
                // z 轴深度   
                m_lineMaterial.SetInt("_ZWrite", 0);
            }
        }

        private void DrawBoxLine()
        {
            m_lineMaterial.SetPass(0);
            GL.PushMatrix();

            float rx = size.x / 2f;
            float ry = size.y / 2f;
            float rz = size.z / 2f;

            var position = gameObject.transform.position;

            p0 = offset + new Vector3(-rx, -ry, rz) + position;
            p1 = offset + new Vector3(rx, -ry, rz) + position;
            p2 = offset + new Vector3(rx, -ry, -rz) + position;
            p3 = offset + new Vector3(-rx, -ry, -rz) + position;
            p4 = offset + new Vector3(-rx, ry, rz) + position;
            p5 = offset + new Vector3(rx, ry, rz) + position;
            p6 = offset + new Vector3(rx, ry, -rz) + position;
            p7 = offset + new Vector3(-rx, ry, -rz) + position;

            DrawLine(p0, p1);
            DrawLine(p1, p2);
            DrawLine(p2, p3);
            DrawLine(p0, p3);
            DrawLine(p4, p5);
            DrawLine(p5, p6);
            DrawLine(p6, p7);
            DrawLine(p4, p7);
            DrawLine(p0, p4);
            DrawLine(p1, p5);
            DrawLine(p2, p6);
            DrawLine(p3, p7);

            GL.PopMatrix();
        }

        private void DrawLine(Vector3 pStart, Vector3 pEnd)
        {
            GL.Begin(GL.LINES);
            GL.Color(color);
            GL.Vertex(pStart);
            GL.Vertex(pEnd);
            GL.End();
        }

        void OnRenderObject()
        {
            if (m_drawing)
            {
                DrawBoxLine();
            }
        }
    }
}

