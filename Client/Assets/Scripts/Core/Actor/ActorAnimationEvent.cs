using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmallUniverse
{
    public class ActorAnimationEvent : MonoBehaviour
    {
		public delegate void AnimationEventHandler();

		public event AnimationEventHandler OnEndHandler;

		public void End_EventHandler()
		{
			if(OnEndHandler != null)
			{
				OnEndHandler();
			}
		}
    }
}

