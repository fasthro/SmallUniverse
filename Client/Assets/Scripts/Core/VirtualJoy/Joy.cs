using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmallUniverse
{
	public enum JoyScreenDirection
	{
		Left,
		Right,
	}

	public enum JoyVirtualShape
	{
		Circle,
		Square,
	}

	public enum JoyTouchEventName
	{
		Init,
		Start,
		Move,
		Up,
	}

	public enum JoyFunc
	{
		MoveJoy,
		AttackJoy,
		SkillJoy_1,
		SkillJoy_2,
		SkillJoy_3,
	}

	[System.Serializable]
    public class Joy
    {
		public delegate void JoyTouchHandler(JoyGesture gesture);
		public delegate void JoyUpdateHandler();
		
		public JoyFunc joyFunc;
		public JoyScreenDirection screenDirection;
		public JoyVirtualShape virtualShape;
		public JoyParame parame;

		public event JoyTouchHandler touchHandler;
		public event JoyUpdateHandler updateHandler;

		// 虚拟摇杆的中心点
		private Vector2 m_virtualCenter;
		private int m_fingerIndex = -1;
		private Vector2 m_touchStartPosition;
		private Vector2 m_touchMovePosition;

		// 是否在虚拟范围内
		private bool m_isCheckInJoy;

		#region touch move

		// 是否正在移动
		private bool m_touchMoveing;
		// 移动的方向
		private Vector2 m_touchMoveDirection;
		// 移动的角度，相对于 Vector2.up
		private float m_touchMoveAngle;
		
		#endregion

		public void Initialize()
		{
			if(screenDirection == JoyScreenDirection.Left)
			{
				m_virtualCenter.x = parame.boundary.x + parame.radius;
				m_virtualCenter.y = Screen.height - parame.boundary.y - parame.radius;
			}
			else if(screenDirection == JoyScreenDirection.Right){
				m_virtualCenter.x = Screen.width - parame.boundary.x - parame.radius;
				m_virtualCenter.y = Screen.height - parame.boundary.y - parame.radius;
			}

			RaiseEvent(JoyTouchEventName.Init, Vector2.zero);
		}

		// 检查目标点是否在虚拟摇杆范围内
		private bool CheckInJoy(Vector2 pos)
		{
			if(virtualShape == JoyVirtualShape.Circle)
			{
				float distance = Vector2.Distance(m_virtualCenter, pos);
				if (distance <= parame.radius)
					return true;
			}
			else if(virtualShape == JoyVirtualShape.Square)
			{
				if(pos.x >= m_virtualCenter.x - parame.radius
				&& pos.x <= m_virtualCenter.x + parame.radius
				&& pos.y >= m_virtualCenter.y - parame.radius
				&& pos.y <= m_virtualCenter.y + parame.radius)
				{
					return true;
				}
			}
			return false;
		}

		// create gesture
		private JoyGesture CreateJoyGesture(JoyTouchEventName eventName, Vector2 endPosition)
		{
			JoyGesture gesture = new JoyGesture();
			gesture.fingerIndex = m_fingerIndex;
			gesture.eventName = eventName;
			gesture.virtualRadius = parame.radius;
			gesture.virtualCenter = m_virtualCenter;
			gesture.touchStartPosition = m_touchStartPosition;
			gesture.toucheMovePosition = m_touchMovePosition;
			gesture.touchEndPosition = endPosition;
			
			gesture.touchMoveing = m_touchMoveing;
			gesture.touchMoveDirection = m_touchMoveDirection;
			gesture.touchMoveAngle = m_touchMoveAngle;

			return gesture;
		}

		private void RaiseEvent(JoyTouchEventName eventName, Vector2 endPosition)
		{
			if(touchHandler != null)
			{
				touchHandler(CreateJoyGesture(eventName, endPosition));
			}
		}

		public void On_Update()
		{
			if(updateHandler != null)
			{
				updateHandler();
			}
		}

		public void On_TouchStart(Gesture gesture)
		{
			var position = ToTouchPoint(gesture.position);

			m_isCheckInJoy = CheckInJoy(position);
			
			if(!m_isCheckInJoy)
			{
				m_fingerIndex = -1;
				return;
			}
			
			m_fingerIndex = gesture.fingerIndex;

			m_touchMoveing = false;

			m_touchStartPosition = position;

			RaiseEvent(JoyTouchEventName.Start, m_touchStartPosition);
		}

		public void On_TouchUp(Gesture gesture)
		{
			if(!m_isCheckInJoy || m_fingerIndex == -1 || gesture.fingerIndex != m_fingerIndex)
				return;

			var position = ToTouchPoint(gesture.position);

			m_fingerIndex = -1;
			m_touchMoveing = false;

			RaiseEvent(JoyTouchEventName.Up, position);
		}

		public void On_TouchMove(Gesture gesture)
		{
			if(!m_isCheckInJoy || m_fingerIndex == -1 || gesture.fingerIndex != m_fingerIndex)
				return;
			
			var position = ToTouchPoint(gesture.position);

			m_touchMovePosition = position;
			m_touchMoveing = true;
			m_touchMoveDirection = (m_touchStartPosition - m_touchMovePosition).normalized;
			m_touchMoveAngle = 180f + Vector2.SignedAngle(m_touchMoveDirection, Vector2.up);

			RaiseEvent(JoyTouchEventName.Move, m_touchMovePosition);
		}

		private Vector2 ToTouchPoint(Vector2 touchPoint)
		{
			Vector2 np = Vector2.zero;
			np.x = touchPoint.x;
			np.y = Screen.height - touchPoint.y;
			return np;
		}
    }
}
