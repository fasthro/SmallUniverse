using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmallUniverse
{
    public class LevelGridAnimation : MonoBehaviour
    {
        // 准备就绪
        private bool m_ready;
        public bool ready
        {
            get
            {
                return m_ready;
            }
        }
        // 运动已经结束
        private bool m_end;
        // 动画曲线
        private AnimationCurve m_curve;
        // 等待运动时间
        private float m_waitTime;
        // 运动总时间
        private float m_totalTime;
        // 运动偏移
        private Vector3 m_offset;

        // 当前运动时间
        private float m_currentTime;
		// 位置变量
        private Vector3 m_position;
        private Vector3 m_targetPosition;

        private MeshRenderer m_meshRender;

		/// <summary>
        /// 设置格子动画
        /// </summary>
        /// <param name="waitTime">等待waitTime时间之后运动</param>
		/// <param name="totalTime">运动总时间</param>
		/// <param name="offset">运动偏移</param>
		/// <param name="curve">曲线</param>
        public void Initialize(float waitTime, float totalTime, Vector3 offset, AnimationCurve curve)
        {
            m_end = false;
            m_ready = true;
            m_waitTime = waitTime;
            m_totalTime = totalTime;
            m_offset = offset;
            m_curve = curve;
            
			m_currentTime = 0;

            m_targetPosition = gameObject.transform.position;
            m_position = m_targetPosition + m_offset;

            gameObject.transform.position = m_position;

            m_meshRender = gameObject.GetComponentInChildren<MeshRenderer>();
            m_meshRender.enabled = false;
        }

        void Update()
        {
            if (m_end || !m_ready)
                return;

            m_waitTime -= Time.deltaTime;
            if (m_waitTime <= 0)
            {
                if(!m_meshRender.enabled)
                    m_meshRender.enabled = true;

                m_currentTime += Time.deltaTime;
                if (m_currentTime < m_totalTime)
                {
                    Vector3 sport = m_curve.Evaluate(m_currentTime / m_totalTime) * m_offset;
                    gameObject.transform.position = m_position + sport;
                }
                else
                {
                    m_ready = false;
                    m_end = true;
                    gameObject.transform.position = m_targetPosition;
                }
            }
        }
    }
}

