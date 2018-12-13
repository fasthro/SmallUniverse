using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmallUniverse.GameEditor.LevelEditor
{
    /// <summary>
    /// 功能类型
    /// </summary>
    public enum GridFunction
    {
        Ground,              // 地面
        Player,                // 玩家
        Monster,            // 怪
        Door,                 // 门
        Trap,                  // 陷阱
        Transfer,            // 传送门
        Decoration,       // 装饰品
    }

    public class LEGrid : MonoBehaviour
    {
        // key
        public string key;
        // prefabGo
        public LEPrefabGo prefabGo;
        // 所在地块
        public int area;
        // 位置
        public Vector3 position;
        // 旋转
        public Vector3 rotationAngle;
        // 格子功能
        public GridFunction function;

        // box line
        private LEBoxLine m_boxLine;

        /// <summary>
        /// 初始化
        /// </summary>
        public void Initialize()
        {
            DrawBoxLineView(false);
        }

        /// <summary>
        /// 画 box line
        /// </summary>
        /// <param name="drawing"></param>
        /// <param name="color"></param>
        public void DrawBoxLineView(bool drawing, Color color)
        {
            if (m_boxLine == null)
            {
                m_boxLine = gameObject.GetComponent<LEBoxLine>();
                if(m_boxLine == null)
                {
                    m_boxLine = gameObject.AddComponent<LEBoxLine>();
                }
            }

            m_boxLine.Draw(drawing);

            if (drawing)
            {
                m_boxLine.SetColor(color);
            }
        }

        /// <summary>
        /// 画 box line
        /// </summary>
        /// <param name="drawing"></param>
        public void DrawBoxLineView(bool drawing)
        {
            if (m_boxLine == null)
            {
                m_boxLine = gameObject.GetComponent<LEBoxLine>();
                if(m_boxLine == null)
                {
                    m_boxLine = gameObject.AddComponent<LEBoxLine>();
                }
            }
            
            m_boxLine.Draw(drawing);
        }


        #region rotate
        public void RotateX()
        {
            rotationAngle.x += 90f;
            transform.localEulerAngles = rotationAngle;
        }

        public void RotateY()
        {
            rotationAngle.y += 90f;
            transform.localEulerAngles = rotationAngle;
        }

        public void RotateZ()
        {
            rotationAngle.z += 90f;
            transform.localEulerAngles = rotationAngle;
        }
        #endregion
    }
}
