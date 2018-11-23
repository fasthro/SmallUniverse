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
        // 运动的最大距离
        private float m_maxDistance;

        // 当前运动时间
        private float m_currentTime;
		// 位置变量
        private Vector3 m_position;
        private Vector3 m_sportPosition;
        private Vector3 m_targetPosition;

        private MeshRenderer m_meshRender;

		/// <summary>
        /// 设置格子动画
        /// </summary>
        /// <param name="waitTime">等待waitTime时间之后运动</param>
		/// <param name="totalTime">运动总时间</param>
		/// <param name="maxDistance">运动最大距离</param>
		/// <param name="curve">曲线</param>
        public void Initialize(float waitTime, float totalTime, float maxDistance, AnimationCurve curve)
        {
            Debug.Log("waitTime : " + waitTime);
            m_end = false;
            m_ready = true;
            m_waitTime = waitTime;
            m_totalTime = totalTime;
            m_maxDistance = maxDistance;
            m_curve = curve;
			
            m_targetPosition = gameObject.transform.position;

            m_position.x = m_targetPosition.x;
            m_position.y = m_targetPosition.y - m_maxDistance;
            m_position.z = m_targetPosition.z;

            m_sportPosition.x = m_targetPosition.x;
            m_sportPosition.y = m_targetPosition.y - m_maxDistance;
            m_sportPosition.z = m_targetPosition.z;

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
                    float sport = m_curve.Evaluate(m_currentTime / m_totalTime) * m_maxDistance;
                    m_sportPosition.y = m_position.y + sport;
                    gameObject.transform.position = m_sportPosition;
                }
                else
                {
                    m_end = true;
                    gameObject.transform.position = m_targetPosition;
                }
            }
        }
    }
}

