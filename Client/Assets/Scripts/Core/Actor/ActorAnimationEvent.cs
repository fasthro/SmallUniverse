using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmallUniverse
{
    public class ActorAnimationEvent : MonoBehaviour
    {
		// 动画事件
		public delegate void AnimationEventHandler();

		// 动画结束事件
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

