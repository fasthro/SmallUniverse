using System;
using UnityEngine;

namespace SmallUniverse.Utils
{
    public class GameFPS : MonoBehaviour
    {
        public enum Direction
        {
            Left_Up,
            Left_Down,
            Right_Up,
            Right_Down,
        }

        [Tooltip("所在屏幕的位置")]
        public Direction direction = Direction.Right_Up;
        [Tooltip("距离屏幕边的距离")]
        public Vector2 boundary = new Vector2(0f, 0f);
        [Tooltip("GUI 尺寸")]
        public Vector2 size = new Vector2(100f, 40f);
        [Tooltip("字体 Size")]
        public int fontSize = 18;
        [Tooltip("更新频率")]
        public float frequency = 0.5f;
        [Tooltip("fps精度")]
        public int nbDecimal = 1;

        private float m_accum;
        private int m_frames;
        private float m_accumTime;
        private Color m_color = Color.white;
        private string m_fps = string.Empty;
        private Rect m_rect;
        private GUIStyle style;
        
        private void Awake()
        {
            if(direction == Direction.Left_Up)
            {
                m_rect = new Rect(boundary.x, boundary.y, size.x, size.y);
            }
            else if(direction == Direction.Left_Down)
            {
                m_rect = new Rect(boundary.x, (float)Screen.height - boundary.y, size.x, size.y);
            }
            else if(direction == Direction.Right_Up)
            {
                m_rect = new Rect((float)Screen.width - boundary.x - size.x, boundary.y, size.x, size.y);
            }
            else if(direction == Direction.Right_Down)
            {
                m_rect = new Rect((float)Screen.width - boundary.x - size.x, (float)Screen.height - boundary.y, size.x, size.y);
            }
        }

        private void Update()
        {
            m_accumTime += Time.deltaTime;
            m_accum += Time.timeScale / Time.deltaTime;
            m_frames++;
            if (m_accumTime >= frequency)
            {
                float num = m_accum / (float)m_frames;
                m_fps = num.ToString("f" + Mathf.Clamp(nbDecimal, 0, 10));
                m_color = ((num >= 20f) ? Color.green : ((num > 10f) ? Color.red : Color.yellow));
                m_accumTime = 0f;
                m_accum = 0f;
                m_frames = 0;
            }
        }

        private void OnGUI()
        {
            if (style == null)
            {
                style = new GUIStyle(GUI.skin.label);
                style.fontStyle = FontStyle.Bold;
                style.fontSize = fontSize;
                style.alignment = TextAnchor.MiddleCenter;
            }

            GUI.Box(m_rect, string.Empty);

            style.normal.textColor = m_color;
            GUI.Button(m_rect, "FPS " + m_fps, style);
        }
    }
}

