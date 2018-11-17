using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmallUniverse
{
    public abstract class VirtualFuncJoyBase
    {
        protected Joy m_joy;

        public virtual void Initialize(Joy joy)
        {
            m_joy = joy;

            m_joy.touchHandler -= TouchHandler;
            m_joy.touchHandler += TouchHandler;

			m_joy.updateHandler -= OnUpdate;
			m_joy.updateHandler += OnUpdate;

            m_joy.keyDownHandler -= OnKeyDown;
            m_joy.keyDownHandler += OnKeyDown;

            m_joy.keyUpHandler -= OnKeyUp;
            m_joy.keyUpHandler += OnKeyUp;
        }

        public virtual void Disable()
        {
            m_joy.touchHandler -= TouchHandler;
			m_joy.updateHandler -= OnUpdate;
            m_joy.keyDownHandler -= OnKeyDown;
            m_joy.keyUpHandler -= OnKeyUp;
        }

        private void TouchHandler(JoyGesture gesture)
        {
			switch(gesture.eventName)
			{
				case JoyTouchEventName.Init:
					OnTouchInit(gesture);
					break;
				case JoyTouchEventName.Start:
					OnTouchStart(gesture);
					break;
				case JoyTouchEventName.Move:
					OnTouchMove(gesture);
					break;	
				case JoyTouchEventName.Up:
					OnTouchUp(gesture);
					break;
			}
        }

		protected virtual void OnUpdate(){}
		protected virtual void OnTouchInit(JoyGesture gesture){}
		protected virtual void OnTouchStart(JoyGesture gesture){}
		protected virtual void OnTouchMove(JoyGesture gesture){}
		protected virtual void OnTouchUp(JoyGesture gesture){}

        protected virtual void OnKeyDown(){}
        protected virtual void OnKeyUp(){}
		

        protected Vector2 CenterToUIPoint(Vector2 center, float uiRadius)
        {
            Vector2 uiPoint = Vector2.zero;

            uiPoint.x = center.x - uiRadius;
            uiPoint.y = center.y - uiRadius;

            return uiPoint;
        }
    }
}
