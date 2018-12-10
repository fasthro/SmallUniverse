using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmallUniverse
{
    public class PoolDelayDespawn : MonoBehaviour
    {
        // 延时
        public float delay;

        private float m_delay;
        private float m_time;
        private bool m_isDespawn;

		void OnEnable()
		{
			m_isDespawn = false;
            m_time = Time.realtimeSinceStartup;
            m_delay = 0;
		}

        void Update()
        {
            if (!m_isDespawn)
            {
                if (m_delay >= delay)
                {
                    m_isDespawn = true;
                    gameObject.Despawn();
                }
                m_delay += Time.realtimeSinceStartup - m_time;
            }
        }
    }
}
