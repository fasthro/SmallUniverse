using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmallUniverse
{
    public class AIDelay : AIBase
    {
		public float delay;

		private float m_delay;

        void Update()
        {
			if(m_actor == null)
				return;

			if(m_actor.actorState == ActorState.None || m_actor.actorState == ActorState.Death)
				return;

			m_delay += Time.deltaTime;

			if(m_delay >= delay)
			{
				if(m_actor.actorState != ActorState.Attack)
				{
					m_delay = 0;
					m_actor.Attack();
				}
			}
        }
    }
}
