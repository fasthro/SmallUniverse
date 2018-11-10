using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmallUniverse
{
	public enum ActorAttributeType
	{
		MoveSpeed, // 移动速度
	}

    public class ActorAttribute
    {
		// 移动速度
		private float m_moveSpeed = 1;

		public float GetAttribute(ActorAttributeType attributeType)
		{
			switch(attributeType)
			{
				case ActorAttributeType.MoveSpeed:
					return m_moveSpeed;
				default:
					return 0;
			}
		}

		public void SetAttribute(ActorAttributeType attributeType, float value)
		{
			switch(attributeType)
			{
				case ActorAttributeType.MoveSpeed:
					m_moveSpeed = value;
					break;
			}
		}
    }
}
